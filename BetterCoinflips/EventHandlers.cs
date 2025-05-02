using System;
using System.Collections.Generic;
using Exiled.API.Features;
using System.Linq;
using BetterCoinflips.Configs;
using BetterCoinflips.Types;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using MEC;
using PlayerRoles;

namespace BetterCoinflips
{
    /// <summary>
    /// Handles various game events related to coin flips and player interactions.
    /// </summary>
    public class EventHandlers
    {
        private static Config Config => Plugin.Instance.Config;
        private static Configs.Translations Translations => Plugin.Instance.Translation;
        private readonly System.Random _random = new();
        private readonly Dictionary<string, int> _respawnCount = new();
        public static Dictionary<string, int> RespawnCount = new();
        public static readonly Dictionary<string, Vector3> InitialSpawnPositions = new();
        public static readonly Dictionary<ushort, int> CoinUses = new();

        // Dictionary of all good coin effect chances with an index
        private readonly Dictionary<int, int> _goodEffectChances = new()
        {
            { 0, Config.RandomCardChance },
            { 1, Config.MedicalKitChance },
            { 2, Config.TpToEscapeChance },
            { 3, Config.HealChance },
            { 4, Config.MoreHpChance },
            { 5, Config.RandomsScpItemChance },
            { 6, Config.RandomGoodEffectChance },
            { 7, Config.WallHackChance },
            { 8, Config.PinkCandyChance },
            { 9, Config.BadRevoChance },
            { 10, Config.SpawnHidChance },
            { 11, Config.ForceRespawnChance },
            { 12, Config.SizeChangeChance },
            { 13, Config.RandomItemChance },
            { 14, Config.AmmoRefillChance },
            { 15, Config.TemporaryGodmodeChance },
            { 16, Config.KeycardUpgradeChance },
            { 17, Config.AllPositiveEffectsChance },
            { 18, Config.RandomHpChance },
            { 19, Config.ThousandHpChance },
            { 20, Config.RandomGeneratorActivationChance },
            { 21, Config.DominoEffectChance },
            { 22, Config.TimeLoopChance },
        };

        // Dictionary of all bad coin effect chances with an index
        private readonly Dictionary<int, int> _badEffectChances = new()
        {
            { 0, Config.HpReductionChance },
            { 1, Config.TpToClassDCellsChance },
            { 2, Config.RandomBadEffectChance },
            { 3, Config.WarheadChance },
            { 4, Config.LightsOutChance },
            { 5, Config.LiveHeChance },
            { 6, Config.TrollFlashChance },
            { 7, Config.ScpTpChance },
            { 8, Config.OneHpLeftChance },
            { 9, Config.PrimedVaseChance },
            { 10, Config.ShitPantsChance },
            { 11, Config.FakeCassieChance },
            { 12, Config.TurnIntoScpChance },
            { 13, Config.InventoryResetChance },
            { 14, Config.ClassSwapChance },
            { 15, Config.InstantExplosionChance },
            { 16, Config.PlayerSwapChance },
            { 17, Config.KickChance },
            { 18, Config.SpectSwapChance },
            { 19, Config.TeslaTpChance },
            { 20, Config.InventorySwapChance },
            { 21, Config.RandomTeleportChance },
            { 22, Config.HandcuffChance },
            { 23, Config.TeleportToSpawnChance },
            { 24, Config.FakeNtfChance },
            { 25, Config.LightZoneDecontaminationChance },
            { 26, Config.RandomTeleportationChance },
            { 27, Config.UpsideDownScaleChance },
            { 28, Config.ZoneDoorLockChance },
            { 29, Config.RandomItemDropChance },
            { 30, Config.WalkingTimeBombChance },
        };

        private readonly Dictionary<string, DateTime> _cooldowns = new();

        /// <summary>
        /// Sends a broadcast message to the specified player.
        /// </summary>
        /// <param name="player">The player to send the message to.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="showHint">Whether to show a hint to the player.</param>
        /// <param name="isTails">Indicates if the message is related to a tails event.</param>
        public static void SendBroadcast(Player player, string message, bool showHint = false, bool isTails = false)
        {
            player.Broadcast(new Exiled.API.Features.Broadcast($"<color=#008000><b>{message}</b></color>", Config.BroadcastTime), true);

            if (showHint && Config.HintDuration > 0)
            {
                var hint = isTails ? Translations.HintMessages.First() : Translations.HintMessages.ElementAt(1);
                player.ShowHint(hint, Config.HintDuration);
            }
        }

        /// <summary>
        /// Handles the coin flip event and applies the appropriate effect based on the result.
        /// </summary>
        public void OnCoinFlip(FlippingCoinEventArgs ev)
        {
            if (IsOnCooldown(ev.Player.RawUserId))
            {
                ev.IsAllowed = false;
                SendBroadcast(ev.Player, Translations.TossOnCooldownMessage);
                Log.Debug($"{ev.Player.Nickname} tried to throw a coin on cooldown.");
                return;
            }

            SetCooldown(ev.Player.RawUserId);

            if (!TryRegisterCoinUses(ev))
                return;

            DecrementCoinUses(ev.Player.CurrentItem.Serial, out var usesLeft);

            Log.Debug($"Is tails: {ev.IsTails}");

            string message;
            if (!ev.IsTails)
            {
                message = ExecuteRandomEffect(_goodEffectChances, CoinFlipEffect.GoodEffects
                    .Select((effect, index) => new { index, effect })
                    .ToDictionary(x => x.index, x => x.effect), ev.Player);
            }
            else
            {
                message = ExecuteRandomEffect(_badEffectChances, CoinFlipEffect.BadEffects
                    .Select((effect, index) => new { index, effect })
                    .ToDictionary(x => x.index, x => x.effect), ev.Player);
            }

            if (usesLeft < 1)
            {
                if (ev.Player.CurrentItem != null)
                    ev.Player.RemoveHeldItem();

                message += Translations.CoinBreaksMessage;
            }

            if (!string.IsNullOrEmpty(message))
                SendBroadcast(ev.Player, message, true, ev.IsTails);
        }

        /// <summary>
        /// Removes default coins from spawning if configured.
        /// </summary>
        public void OnSpawningItem(SpawningItemEventArgs ev)
        {
            if (Config.DefaultCoinsAmount == 0 || ev.Pickup.Type != ItemType.Coin)
                return;

            Log.Debug($"Removed a coin, coins left to remove {Config.DefaultCoinsAmount}");
            ev.IsAllowed = false;
            Config.DefaultCoinsAmount--;
        }

        /// <summary>
        /// Handles the locker filling event to remove or replace coins.
        /// </summary>
        public void OnFillingLocker(FillingLockerEventArgs ev)
        {
            if (ev.Pickup.Type == ItemType.Coin && Config.DefaultCoinsAmount != 0)
            {
                Log.Debug($"Removed a locker coin, coins left to remove {Config.DefaultCoinsAmount}");
                ev.IsAllowed = false;
                Config.DefaultCoinsAmount--;
            }
            else if (ev.Pickup.Type == Config.ItemToReplace.ElementAt(0).Key
                     && Config.ItemToReplace.ElementAt(0).Value != 0)
            {
                Log.Debug($"Placed a coin, coins left to place: {Config.ItemToReplace.ElementAt(0).Value}. Replaced item: {ev.Pickup.Type}");
                ev.IsAllowed = false;
                Pickup.CreateAndSpawn(ItemType.Coin, ev.Pickup.Position, new Quaternion());
                Config.ItemToReplace[Config.ItemToReplace.ElementAt(0).Key]--;
            }
        }

        /// <summary>
        /// Handles the player spawn event to adjust player size and track initial positions.
        /// </summary>
        public void OnPlayerSpawned(SpawningEventArgs ev)
        {
            if (Config.SizeReductionBehavior == 2 && _respawnCount.ContainsKey(ev.Player.UserId))
            {
                if (ev.Player.Scale.y < 1)
                {
                    float scaleFactorY = ev.Player.Scale.y + Config.GrowthFrequency;
                    scaleFactorY = Math.Min(scaleFactorY, 1.0f);
                    ev.Player.Scale = new Vector3(scaleFactorY, scaleFactorY, scaleFactorY);
                }
            }

            if (ev.Player.Role.Team == Team.FoundationForces || ev.Player.Role.Team == Team.ChaosInsurgency)
            {
                InitialSpawnPositions[$"{ev.Player.UserId}_respawn_{ev.Player.Role.Team}"] = ev.Position;
                Log.Debug($"Stored team respawn for {ev.Player.Nickname}: {ev.Position} (Team: {ev.Player.Role.Team})");
            }
            else
            {
                // For other roles, always update or set the initial spawn position
                InitialSpawnPositions[ev.Player.UserId] = ev.Position;
                Log.Debug($"Stored/Updated spawn for {ev.Player.Nickname}: {ev.Position} (Role: {ev.Player.Role.Type})");
            }
        }

        /// <summary>
        /// Handles the player death event to reset or adjust player size and trigger effects.
        /// </summary>
        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (Config.SizeReductionBehavior == 0)
            {
                ev.Player.Scale = new Vector3(1, 1, 1);
            }
            else if (Config.SizeReductionBehavior == 2)
            {
                if (ev.Player.Scale.y < 1)
                {
                    if (!_respawnCount.ContainsKey(ev.Player.UserId))
                    {
                        _respawnCount[ev.Player.UserId] = 0;
                    }

                    _respawnCount[ev.Player.UserId]++;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlers"/> class.
        /// </summary>
        public EventHandlers()
        {
            if (Config.RandomCoinInterval > 0)
                Timing.RunCoroutine(RandomCoinRoutine());
        }

        /// <summary>
        /// Coroutine that periodically gives a random player a coin.
        /// </summary>
        /// <returns>An enumerator for the coroutine.</returns>
        private IEnumerator<float> RandomCoinRoutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(Config.RandomCoinInterval * 60);
                GiveRandomCoin();
            }
        }

        /// <summary>
        /// Gives a random player a coin if they are eligible.
        /// </summary>
        public void GiveRandomCoin()
        {
            if (Config.RandomCoinInterval <= 0) return;

            var eligiblePlayers = Player.List.Where(p => !p.IsScp && p.IsAlive).ToList();
            if (!eligiblePlayers.Any()) return;

            var randomPlayer = eligiblePlayers[UnityEngine.Random.Range(0, eligiblePlayers.Count)];

            if (randomPlayer.Items.Count() < 8)
            {
                randomPlayer.AddItem(ItemType.Coin);
                SendBroadcast(randomPlayer, Translations.RandomCoinMessage);
            }
            else
            {
                Pickup.CreateAndSpawn(ItemType.Coin, randomPlayer.Position, Quaternion.identity);
                SendBroadcast(randomPlayer, Translations.RandomCoinDropMessage);
            }
        }
        private bool IsOnCooldown(string userId) => _cooldowns.TryGetValue(userId, out var lastUsed) && (DateTime.UtcNow - lastUsed).TotalSeconds < Config.CoinCooldown;

        private void SetCooldown(string userId) => _cooldowns[userId] = DateTime.UtcNow;

        /// <summary>
        /// Attempts to register a coin.
        /// </summary>
        /// <returns></returns>
        private bool TryRegisterCoinUses(FlippingCoinEventArgs ev)
        {
            if (CoinUses.ContainsKey(ev.Player.CurrentItem.Serial))
                return true;

            var uses = _random.Next(Config.MinMaxDefaultCoins[0], Config.MinMaxDefaultCoins[1]);
            CoinUses.Add(ev.Player.CurrentItem.Serial, uses);
            Log.Debug($"Registered a coin, Uses Left: {uses}");

            if (uses < 1)
            {
                CoinUses.Remove(ev.Player.CurrentItem.Serial);
                Log.Debug("Removed the coin");
                ev.Player.RemoveHeldItem();
                SendBroadcast(ev.Player, Translations.CoinNoUsesMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handles the coin flip event.
        /// </summary>
        private void DecrementCoinUses(ushort serial, out int usesLeft)
        {
            CoinUses[serial]--;
            usesLeft = CoinUses[serial];
            Log.Debug($"Uses Left: {usesLeft}");
        }

        /// <summary>
        /// Executes a random effect based on the provided chances and effects.
        /// </summary>
        private string ExecuteRandomEffect(Dictionary<int, int> chances, IReadOnlyDictionary<int, CoinFlipEffect> effects, Player player)
        {
            var totalChance = chances.Values.Sum();
            var randomNum = _random.Next(1, totalChance + 1);

            foreach (var kvp in chances)
            {
                int key = kvp.Key;
                int chance = kvp.Value;

                if (randomNum <= chance)
                {
                    effects.TryGetValue(key, out var effect);
                    effect?.Execute(player);
                    Log.Debug($"Effect executed: {key}");
                    return effect?.Message ?? string.Empty;
                }

                randomNum -= chance;
            }

            return string.Empty;
        }
    }
}