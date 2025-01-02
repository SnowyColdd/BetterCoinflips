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
        private readonly System.Random _rd = new();
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

        private readonly Dictionary<string, DateTime> _cooldownDict = new();

        /// <summary>
        /// Sends a broadcast message to the specified player.
        /// </summary>
        /// <param name="pl">The player to send the message to.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="showHint">Whether to show a hint to the player.</param>
        /// <param name="isTails">Indicates if the message is related to a tails event.</param>
        public static void SendBroadcast(Player pl, string message, bool showHint = false, bool isTails = false)
        {
            pl.Broadcast(new Exiled.API.Features.Broadcast($"<color=#008000><b>{message}</b></color>", Config.BroadcastTime), true);

            if (showHint && Config.HintDuration > 0)
            {
                pl.ShowHint(isTails ? Translations.HintMessages.First() : Translations.HintMessages.ElementAt(1), Config.HintDuration);
            }
        }

        /// <summary>
        /// Handles the coin flip event and applies the appropriate effect based on the result.
        /// </summary>
        /// <param name="ev">The event arguments for the coin flip</param>
        public void OnCoinFlip(FlippingCoinEventArgs ev)
        {
            // Broadcast message
            string message = "";
            // Used to remove the coin if uses run out, since they are checked before executing the effect
            bool helper = false;

            // Check if player is on cooldown
            bool flag = _cooldownDict.ContainsKey(ev.Player.RawUserId)
                        && (DateTime.UtcNow - _cooldownDict[ev.Player.RawUserId]).TotalSeconds < Config.CoinCooldown;
            if (flag)
            {
                ev.IsAllowed = false;
                SendBroadcast(ev.Player, Translations.TossOnCooldownMessage);
                Log.Debug($"{ev.Player.Nickname} tried to throw a coin on cooldown.");
                return;
            }

            // Set cooldown for player
            _cooldownDict[ev.Player.RawUserId] = DateTime.UtcNow;

            // Check if coin has registered uses
            if (!CoinUses.ContainsKey(ev.Player.CurrentItem.Serial))
            {
                CoinUses.Add(ev.Player.CurrentItem.Serial, _rd.Next(Config.MinMaxDefaultCoins[0], Config.MinMaxDefaultCoins[1]));
                Log.Debug($"Registered a coin, Uses Left: {CoinUses[ev.Player.CurrentItem.Serial]}");
                
                // Check if the newly registered coin has no uses
                if (CoinUses[ev.Player.CurrentItem.Serial] < 1)
                {
                    //remove the coin from the uses list
                    CoinUses.Remove(ev.Player.CurrentItem.Serial);
                    Log.Debug("Removed the coin");
                    if (ev.Player.CurrentItem != null)
                    {
                        ev.Player.RemoveHeldItem();
                    }
                    SendBroadcast(ev.Player, Translations.CoinNoUsesMessage);
                    return;
                }
            }

            // Decrement coin uses
            CoinUses[ev.Player.CurrentItem.Serial]--;
            Log.Debug($"Uses Left: {CoinUses[ev.Player.CurrentItem.Serial]}");

            // Check if uses that were already registered have been set to 0 to remove the coin after executing the effect
            if (CoinUses[ev.Player.CurrentItem.Serial] < 1)
            {
                helper = true;
            }

            Log.Debug($"Is tails: {ev.IsTails}");

            if (!ev.IsTails)
            {
                int totalChance = _goodEffectChances.Values.Sum();
                int randomNum = _rd.Next(1, totalChance + 1);
                int headsEvent = 2;

                // Determine heads event
                foreach (KeyValuePair<int, int> kvp in _goodEffectChances)
                {
                    if (randomNum <= kvp.Value)
                    {
                        headsEvent = kvp.Key;
                        break;
                    }

                    randomNum -= kvp.Value;
                }

                Log.Debug($"headsEvent = {headsEvent}");

                // Execute the effect
                var effect = CoinFlipEffect.GoodEffects[headsEvent];
                effect.Execute(ev.Player);
                message = effect.Message;
            }
            else
            {
                int totalChance = _badEffectChances.Values.Sum();
                int randomNum = _rd.Next(1, totalChance + 1);
                int tailsEvent = 13;

                // Detarmine tails event
                foreach (KeyValuePair<int, int> kvp in _badEffectChances)
                {
                    if (randomNum <= kvp.Value)
                    {
                        tailsEvent = kvp.Key;
                        break;
                    }

                    randomNum -= kvp.Value;
                }

                Log.Debug($"tailsEvent = {tailsEvent}");

                // Execute the effect
                var effect = CoinFlipEffect.BadEffects[tailsEvent];
                effect.Execute(ev.Player);
                message = effect.Message;
            }

            // If the coin has 0 uses remove it
            if (helper)
            {
                if (ev.Player.CurrentItem != null)
                {
                    ev.Player.RemoveHeldItem();
                }
                message += Translations.CoinBreaksMessage;
            }

            if (!string.IsNullOrEmpty(message))
            {
                SendBroadcast(ev.Player, message, true, ev.IsTails);
            }
        }

        /// <summary>
        /// Handles the item spawning event to remove default coins.
        /// </summary>
        /// <param name="ev">The event arguments for item spawning.</param>
        public void OnSpawningItem(SpawningItemEventArgs ev)
        {
            if (Config.DefaultCoinsAmount != 0 && ev.Pickup.Type == ItemType.Coin)
            {
                Log.Debug($"Removed a coin, coins left to remove {Config.DefaultCoinsAmount}");
                ev.IsAllowed = false;
                Config.DefaultCoinsAmount--;
            }
        }

        /// <summary>
        /// Handles the locker filling event to remove or replace coins.
        /// </summary>
        /// <param name="ev">The event arguments for locker filling.</param>
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
        /// <param name="ev">The event arguments for player spawning.</param>
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
        /// <param name="ev">The event arguments for player death.</param>
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
            {
                Timing.RunCoroutine(RandomCoinRoutine());
            }
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
    }
}