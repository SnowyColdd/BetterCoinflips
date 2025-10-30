using System;
using System.Collections.Generic;
using System.Linq;
using BetterCoinflips.Configs;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Firearms.Attachments;
using MEC;
using PlayerRoles;
using Respawning;
using UnityEngine;
using Player = Exiled.API.Features.Player;

namespace BetterCoinflips.Types
{
    /// <summary>
    /// Represents a coin flip effect with an associated message and execution action.
    /// </summary>
    public class CoinFlipEffect
    {
        private static Config Config => Plugin.Instance.Config;
        private static Configs.Translations Translations => Plugin.Instance.Translation;
        private static readonly System.Random Rd = new();
        
        /// <summary>
        /// Action to execute when the effect is triggered.
        /// </summary>
        public Action<Player> Execute { get; set; }

        /// <summary>
        /// Message associated with the effect.
        /// </summary>
        public string Message { get; set; }

        public CoinFlipEffect(string message, Action<Player> execute)
        {
            Execute = execute;
            Message = message;
        }

        private static readonly Dictionary<string, string> _scpNames = new()
        {
            { "1 7 3", "SCP-173"},
            { "9 3 9", "SCP-939"},
            { "0 9 6", "SCP-096"},
            { "0 7 9", "SCP-079"},
            { "0 4 9", "SCP-049"},
            { "1 0 6", "SCP-106"}
        };

        /// <summary>
        /// Checks if a player is null or dead and logs a warning if so.
        /// </summary>
        private static bool IsPlayerInvalid(Player player, string warningMessage)
        {
            if (player == null || !player.IsAlive)
            {
                Log.Warn(warningMessage);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds an item to player inventory or spawns it as pickup if inventory is full.
        /// </summary>
        private static void AddOrSpawnItem(Player player, ItemType itemType)
        {
            if (player.Items.Count() < 8)
                player.AddItem(itemType);
            else
                Pickup.CreateAndSpawn(itemType, player.Position, Quaternion.identity);
        }

        /// <summary>
        /// Creates an item and either adds to inventory or spawns as pickup.
        /// </summary>
        private static T CreateAndAddItem<T>(Player player, ItemType itemType, Action<T> configure = null) where T : Item
        {
            T item = (T)Item.Create(itemType);
            configure?.Invoke(item);

            if (player.Items.Count() < 8)
                player.AddItem(item);
            else
                item.CreatePickup(player.Position);

            return item;
        }

        // GoodEffects list
        public static List<CoinFlipEffect> GoodEffects { get; } = new()
        {
            // 0: Gives player a random card.
            new CoinFlipEffect(Translations.RandomCardMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to give a random keycard to a null or dead player"))
                        return;

                    ItemType[] keycards = {
                        ItemType.KeycardJanitor, ItemType.KeycardScientist, ItemType.KeycardResearchCoordinator,
                        ItemType.KeycardFacilityManager, ItemType.KeycardGuard, ItemType.KeycardMTFOperative,
                        ItemType.KeycardMTFCaptain, ItemType.KeycardContainmentEngineer, ItemType.KeycardChaosInsurgency,
                        ItemType.KeycardZoneManager, ItemType.KeycardMTFPrivate, ItemType.KeycardO5, ItemType.SurfaceAccessPass
                    };
                    ItemType randomKeycard = keycards[Rd.Next(keycards.Length)];

                    AddOrSpawnItem(player, randomKeycard);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while giving random keycard: {ex.Message}");
                }
            }),

            // 1: Spawns a medkit and painkillers for the player.
            new CoinFlipEffect(Translations.MediKitMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn medkit and painkillers for a null or dead player."))
                        return;

                    if (player.Items.Count() < 7)
                    {
                        player.AddItem(ItemType.Medkit);
                        player.AddItem(ItemType.Painkillers);
                    }
                    else
                    {
                        Pickup.CreateAndSpawn(ItemType.Medkit, player.Position, new Quaternion());
                        Pickup.CreateAndSpawn(ItemType.Painkillers, player.Position, new Quaternion());
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning medkit and painkillers: {ex.Message}");
                }
            }),

            // 2: Teleports the player to the escape primary door.
            new CoinFlipEffect(Translations.TpToEscapeMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to teleport a null or dead player to escape."))
                        return;

                    player.Teleport(Door.Get(DoorType.EscapePrimary));
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while teleporting to escape: {ex.Message}");
                }
            }),

            // 3: Heals the player's full HP and stamina reset.
            new CoinFlipEffect(Translations.MagicHealMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to heal a null or dead player."))
                        return;

                    player.Health = player.MaxHealth;
                    player.ResetStamina();
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while healing player: {ex.Message}");
                }
            }),

            // 4: Increases the player's health by 10%.
            new CoinFlipEffect(Translations.HealthIncreaseMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to increase health of a null or dead player."))
                        return;

                    player.Health *= 1.1f;
                }
                catch (Exception ex)
                {
                   Log.Error($"Error while increasing health: {ex.Message}");
                }
            }),

            // 5: Spawns random SCP item for the player.
            new CoinFlipEffect(Translations.RandomsScpItem, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn random SCP item for a null or dead player."))
                        return;

                    ItemType[] scpItems = {
                        ItemType.SCP018, ItemType.SCP207, ItemType.AntiSCP207, ItemType.SCP268,
                        ItemType.SCP500, ItemType.SCP330, ItemType.SCP1853, ItemType.SCP1576,
                        ItemType.SCP244a, ItemType.SCP244b, ItemType.SCP1344, ItemType.GunSCP127
                    };
                    ItemType randomScpItem = scpItems[Rd.Next(scpItems.Length)];

                    AddOrSpawnItem(player, randomScpItem);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning random SCP item: {ex.Message}");
                }
            }),

            // 6: Applies a random good effect to the player.
            new CoinFlipEffect(Translations.RandomGoodEffectMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to apply a random good effect to a null or dead player."))
                        return;

                    if (!Config.GoodEffects.Any())
                    {
                        Log.Warn("No good effects available.");
                        return;
                    }

                    var effect = Config.GoodEffects.ToList().RandomItem();
                    player.EnableEffect(effect, 5, true);
                    Log.Debug($"Chosen random effect: {effect}");
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while applying random good effect: {ex.Message}");
                }
            }),

            // 7: Gives player SCP-1344 wall hack effect for 15 seconds.
            new CoinFlipEffect(Translations.WallHackMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn give SCP-1344 for a null or dead player."))
                        return;

                    player.EnableEffect(EffectType.Scp1344, 15f, true);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while giving wall hack effect: {ex.Message}");
                }
            }),

            // 8: Spawns pink candy (SCP-330) for the player.
            new CoinFlipEffect(Translations.PinkCandyMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn pink candy for a null or dead player."))
                        return;

                    CreateAndAddItem<Scp330>(player, ItemType.SCP330, candy =>
                        candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink));
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning pink candy: {ex.Message}");
                }
            }),

            // 9: Spawns a customized revolver with attachments for the player.
            new CoinFlipEffect(Translations.BadRevoMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn customized revolver for a null or dead player."))
                        return;

                    CreateAndAddItem<Firearm>(player, ItemType.GunRevolver, revo =>
                        revo.AddAttachment(new[] {
                            AttachmentName.CylinderMag7,
                            AttachmentName.ShortBarrel,
                            AttachmentName.ScopeSight,
                        }));
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning customized revolver: {ex.Message}");
                }
            }),

            // 10: Spawns a random SpecialWeapon for the player.
            new CoinFlipEffect(Translations.RandomSpecialWeaponMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn random SpecialWeapon for a null or dead player."))
                        return;

                    ItemType[] specialWeapons = {
                        ItemType.Jailbird, ItemType.ParticleDisruptor, ItemType.MicroHID
                    };
                    ItemType randomSpecialWeapon = specialWeapons[Rd.Next(specialWeapons.Length)];

                    AddOrSpawnItem(player, randomSpecialWeapon);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning random SpecialWeapon: {ex.Message}");
                }
            }),

            // 11: Forces a respawn wave of the team that has more ticketes.
            new CoinFlipEffect(Translations.ForceRespawnMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to force respawn wave for a null or dead player."))
                        return;

                    Respawn.ForceWave(WaveManager.Waves.RandomItem());
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while forcing respawn wave: {ex.Message}");
                }
            }),

            // 12: Changes the player's size.
            new CoinFlipEffect(Translations.SizeChangeMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to change size of a null or dead player."))
                        return;

                    player.Scale = new Vector3(1.13f, 0.5f, 1.13f);
                    // Reset respawn count when size change effect is applied.
                    if (!EventHandlers.RespawnCount.ContainsKey(player.UserId))
                    {
                        EventHandlers.RespawnCount[player.UserId] = 0;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while changing player size: {ex.Message}");
                }
            }),

            // 13: Spawns a random item for the player.
            new CoinFlipEffect(Translations.RandomItemMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn random item for a null or dead player."))
                        return;

                    var randomItem = Config.ItemsToGive.ToList().RandomItem();

                    AddOrSpawnItem(player, randomItem);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning random item: {ex.Message}");
                }
            }),

            // 14: Refills all ammo and charges MicroHID.
            new CoinFlipEffect(Translations.AmmoRefillMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to refill ammo for a null or dead player."))
                        return;

                    Dictionary<ItemType, Dictionary<AmmoType, ushort>> armorAmmoLimits = new()
                    {
                        {
                            ItemType.None,
                            new Dictionary<AmmoType, ushort>
                            {
                                { AmmoType.Nato9, 40 },
                                { AmmoType.Nato556, 40 },
                                { AmmoType.Nato762, 40 },
                                { AmmoType.Ammo12Gauge, 8 },
                                { AmmoType.Ammo44Cal, 8 }
                            }
                        },
                        {
                            ItemType.ArmorLight,
                            new Dictionary<AmmoType, ushort>
                            {
                                { AmmoType.Nato9, 70 },
                                { AmmoType.Nato556, 40 },
                                { AmmoType.Nato762, 40 },
                                { AmmoType.Ammo12Gauge, 14 },
                                { AmmoType.Ammo44Cal, 18 }
                            }
                        },
                        {
                            ItemType.ArmorCombat,
                            new Dictionary<AmmoType, ushort>
                            {
                                { AmmoType.Nato9, 170 },
                                { AmmoType.Nato556, 120 },
                                { AmmoType.Nato762, 120 },
                                { AmmoType.Ammo12Gauge, 54 },
                                { AmmoType.Ammo44Cal, 48 }
                            }
                        },
                        {
                            ItemType.ArmorHeavy,
                            new Dictionary<AmmoType, ushort>
                            {
                                { AmmoType.Nato9, 210 },
                                { AmmoType.Nato556, 200 },
                                { AmmoType.Nato762, 200 },
                                { AmmoType.Ammo12Gauge, 74 },
                                { AmmoType.Ammo44Cal, 68 }
                            }
                        }
                    };

                    var armor = player.Items.FirstOrDefault(x =>
                        x.Type == ItemType.ArmorLight ||
                        x.Type == ItemType.ArmorCombat ||
                        x.Type == ItemType.ArmorHeavy);

                    var armorsCount = player.Items.Count(x =>
                        x.Type == ItemType.ArmorLight ||
                        x.Type == ItemType.ArmorCombat ||
                        x.Type == ItemType.ArmorHeavy);

                    var ammoLimits = armorsCount > 1
                        ? armorAmmoLimits[ItemType.None]
                        : armorAmmoLimits[armor?.Type ?? ItemType.None];

                    foreach (var ammoLimit in ammoLimits)
                    {
                        player.SetAmmo(ammoLimit.Key, ammoLimit.Value);
                    }

                    foreach (var item in player.Items)
                    {
                        if (item is MicroHid microHid)
                        {
                            microHid.Energy = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while refilling ammo: {ex.Message}");
                }
            }),

            // 15: Gives temporary godmode.
            new CoinFlipEffect(Translations.TemporaryGodmodeMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to give temporary godmode to a null or dead player."))
                        return;

                    player.IsGodModeEnabled = true;
                    Timing.CallDelayed(5f, () =>
                    {
                        if (player?.IsAlive == true)
                            player.IsGodModeEnabled = false;
                    });
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while giving temporary godmode: {ex.Message}");
                }
            }),

            // 16: Upgrade keycard in inventory.
            new CoinFlipEffect(Translations.KeycardUpgradedMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to upgrade keycard for a null or dead player."))
                        return;

                    var keycards = player.Items.Where(item => item.Type.ToString().Contains("Keycard")).ToList();

                    if (!keycards.Any())
                    {
                        EventHandlers.SendBroadcast(player, Translations.NoKeycardMessage);
                        return;
                    }

                    var cardToUpgrade = keycards.Count == 1 ? keycards.First() : keycards[Rd.Next(keycards.Count)];

                    ItemType newCard = cardToUpgrade.Type switch
                    {
                        ItemType.KeycardJanitor => ItemType.SurfaceAccessPass,
                        ItemType.KeycardScientist => ItemType.KeycardResearchCoordinator,
                        ItemType.KeycardResearchCoordinator => ItemType.KeycardFacilityManager,
                        ItemType.KeycardGuard => ItemType.KeycardMTFOperative,
                        ItemType.KeycardMTFOperative => ItemType.KeycardMTFCaptain,
                        ItemType.KeycardContainmentEngineer => ItemType.KeycardFacilityManager,
                        ItemType.KeycardMTFCaptain => ItemType.KeycardO5,
                        _ => ItemType.KeycardO5
                    };

                    player.RemoveItem(cardToUpgrade);
                    player.AddItem(newCard);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while upgrading keycard: {ex.Message}");
                }
            }),

            // 17: Applies all positive effects to the player for 60 seconds.
            new CoinFlipEffect(Translations.AllPositiveEffectsMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to apply all positive effects to a null or dead player."))
                        return;

                    List<EffectType> activeEffects = new();
                    foreach (var effect in Config.GoodEffects)
                    {
                        player.EnableEffect(effect, 60, true);
                        activeEffects.Add(effect);
                    }

                    Timing.CallDelayed(60f, () =>
                    {
                        if (player?.IsAlive == true)
                        {
                            foreach (var effect in activeEffects)
                            {
                                player.DisableEffect(effect);
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while applying all positive effects: {ex.Message}");
                }
            }),

            // 18: Gives to player random HP.
            new CoinFlipEffect(Translations.RandomHpMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to give random HP to a null or dead player."))
                        return;

                    int randomHp = Rd.Next(1, 151);
                    player.Health = randomHp;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while giving random HP: {ex.Message}");
                }
            }),

            // 19: Grants the player 1000 HP for a configurable duration.
            new CoinFlipEffect(Translations.ThousandHpMessage.Replace("{duration}", Config.ThousandHpDuration.ToString()), player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to grant 1000 HP to a null or dead player."))
                        return;

                    float originalHealth = player.Health;
                    player.Health = 1000;

                    // Revert HP back to original after the duration.
                    Timing.CallDelayed(Config.ThousandHpDuration, () =>
                    {
                        if (player?.IsAlive == true)
                        {
                            player.Health = Math.Min(originalHealth, player.Health);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while granting 1000 HP: {ex.Message}");
                }
            }),

            // 20: Activating a random generator.
            new CoinFlipEffect(Translations.RandomGeneratorActivationMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to activate a random generator for a null or dead player."))
                        return;

                    // Logic to activate a random generator.
                    var generators = Generator.List.Where(x => !x.IsEngaged).ToList();
                    if (generators.Any())
                    {
                        var randomGenerator = generators[Rd.Next(generators.Count)];
                        randomGenerator.IsEngaged = true;
                        Log.Info($"Activated generator at {randomGenerator.Position}");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while activating random generator: {ex.Message}");
                }
            }),

            // 21: Domino effect.
            new CoinFlipEffect(Translations.DominoEffectMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to apply domino effect for a null or dead player."))
                        return;

                    foreach (var nearbyPlayer in Player.List.Where(x =>
                        x.IsAlive && x != player &&
                        Vector3.Distance(x.Position, player.Position) <= 10f))
                    {
                        var effect = Config.GoodEffects.ToList().RandomItem();
                        nearbyPlayer.EnableEffect(effect, 10f, true);
                        EventHandlers.SendBroadcast(nearbyPlayer, Translations.DominoEffectReceivedMessage);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while applying domino effect: {ex.Message}");
                }
            }),

            // 22: TimeLoop effect.
            new CoinFlipEffect(Translations.TimeLoopMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to create time loop for a null or dead player."))
                        return;

                    Dictionary<Player, Vector3> playerPositions = new Dictionary<Player, Vector3>();

                    if (Config.TeleportAllPlayersOnCoinFlip)
                    {
                        foreach (var p in Player.List.Where(x =>
                            x.IsAlive && 
                            x.Role.Type != RoleTypeId.Spectator &&
                            x.Role.Type != RoleTypeId.Scp079))
                        {
                            playerPositions.Add(p, p.Position);
                            EventHandlers.SendBroadcast(p, Translations.TimeLoopTeleportingMessage);
                        }
                    }
                    else
                    {
                        playerPositions.Add(player, player.Position);
                    }

                    Timing.CallDelayed(10f, () =>
                    {
                        foreach (var kvp in playerPositions)
                        {
                            if (kvp.Key?.IsAlive == true)
                            {
                                kvp.Key.Teleport(kvp.Value);
                                EventHandlers.SendBroadcast(kvp.Key,
                                    Config.TeleportAllPlayersOnCoinFlip
                                        ? Translations.TimeLoopAllPlayersMessage
                                        : Translations.TimeLoopSinglePlayerTeleportedMessage);

                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while creating time loop: {ex.Message}");
                }
            }),
        };

        // BadEffects list
        public static List<CoinFlipEffect> BadEffects = new()
        {
            // 0: Reduces player's health by 30%.
            new CoinFlipEffect(Translations.HpReductionMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to reduce health of a null or dead player."))
                        return;

                    if ((int) player.Health == 1)
                        player.Kill(DamageType.CardiacArrest);
                    else
                        player.Health *= 0.7f;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while reducing health: {ex.Message}");
                }
            }),

            // 1: Teleports the player to the class D cells.
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.TpToClassDCellsAfterWarheadMessage : Translations.TpToClassDCellsMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to teleport a null or dead player to class D cells."))
                        return;

                    player.DropHeldItem();
                    player.Teleport(Door.Get(DoorType.PrisonDoor));

                    if (Warhead.IsDetonated)
                    {
                        player.Kill(DamageType.Decontamination);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while teleporting to class D cells: {ex.Message}");
                }
            }),

            // 2: Applies a random bad effect to the player.
            new CoinFlipEffect(Translations.RandomBadEffectMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to apply a random bad effect to a null or dead player."))
                        return;

                    var effect = Config.BadEffects.ToList().RandomItem();
                
                    // Prevents players from staying in PD infinitely.
                    if (effect == EffectType.PocketCorroding)
                        player.EnableEffect(EffectType.PocketCorroding);
                    else
                        player.EnableEffect(effect, 5, true);

                    Log.Debug($"Chosen random effect: {effect}");
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while applying random bad effect: {ex.Message}");
                }
            }),

            // 3: Starts or stops the warhead based on its state.
            new CoinFlipEffect(Warhead.IsDetonated || !Warhead.IsInProgress ? Translations.WarheadStartMessage : Translations.WarheadStopMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to toggle warhead for a null or dead player."))
                        return;

                    if (Warhead.IsDetonated || !Warhead.IsInProgress)
                        Warhead.Start();
                    else
                        Warhead.Stop();
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while toggling warhead: {ex.Message}");
                }
            }),

            // 4: Turns off all lights.
            new CoinFlipEffect(Translations.LightsOutMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to turn off lights for a null or dead player."))
                        return;

                    Map.TurnOffAllLights(Config.MapBlackoutTime);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while turning off lights: {ex.Message}");
                }
            }),

            // 5: Spawns a live HE grenade.
            new CoinFlipEffect(Translations.LiveGrenadeMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn live grenade for a null or dead player."))
                        return;

                    ExplosiveGrenade grenade = (ExplosiveGrenade) Item.Create(ItemType.GrenadeHE);
                    grenade.FuseTime = (float) Config.LiveGrenadeFuseTime;
                    grenade.SpawnActive(player.Position + Vector3.up, player);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while spawning live grenade: {ex.Message}");
                }
            }),

            // 6: Spawns a flash grenade with a short fuse time, sets the flash owner to the player so that it hopefully blinds people.
            new CoinFlipEffect(Translations.TrollFlashMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn flash grenade for a null or dead player."))
                        return;

                    FlashGrenade flash = (FlashGrenade) Item.Create(ItemType.GrenadeFlash, player);
                    flash.FuseTime = 1f;
                    flash.SpawnActive(player.Position);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while spawning flash grenade: {ex.Message}");
                }
            }),

            // 7: Teleports the player to a random SCP or inflicts damage if no SCPs exist.
            new CoinFlipEffect(Player.Get(Side.Scp).Any(x => x.Role.Type != RoleTypeId.Scp079) ? Translations.TpToRandomScpMessage : Translations.SmallDamageMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to teleport a null or dead player to random SCP."))
                        return;

                    var scpPlayers = Player.Get(Side.Scp).Where(x => x.Role.Type != RoleTypeId.Scp079).ToList();
                    if (scpPlayers.Any())
                    {
                        Player scpPlayer = scpPlayers.RandomItem();
                        player.Position = scpPlayer.Position;
                        return;
                    }
                    player.Hurt(15);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while teleporting to random SCP: {ex.Message}");
                }
            }),

            // 8: Sets player hp to 1 or kills if it was already 1.
            new CoinFlipEffect(Translations.HugeDamageMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to set HP to 1 for a null or dead player."))
                        return;

                    if ((int) player.Health == 1)
                        player.Kill(DamageType.CardiacArrest);
                    else
                        player.Health = 1;
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while setting player HP to 1: {ex.Message}");
                }
            }),

            // 9: Spawns a primed SCP-244 vase for the player.
            new CoinFlipEffect(Translations.PrimedVaseMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn primed SCP-244 vase for a null or dead player."))
                        return;

                    Scp244 vase = (Scp244)Item.Create(ItemType.SCP244a);
                    vase.Primed = true;
                    vase.CreatePickup(player.Position);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while spawning primed SCP-244 vase: {ex.Message}");
                }
            }),

            // 10: Spawns a tantrum on the player Keywords: shit spawn create.
            new CoinFlipEffect(Translations.ShitPantsMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to place tantrum for a null or dead player."))
                        return;

                    player.PlaceTantrum();
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while placing tantrum: {ex.Message}");
                }
            }),

            // 11: Broadcasts a fake SCP termination message.
            new CoinFlipEffect(Translations.FakeScpKillMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to broadcast fake SCP termination message for a null or dead player."))
                        return;

                    var scpName = _scpNames.ToList().RandomItem();
                    Cassie.MessageTranslated(
                        $"scp {scpName.Key} successfully terminated by automatic security system",
                        $"{scpName.Value} successfully terminated by Automatic Security System.");
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while broadcasting fake SCP termination message: {ex.Message}");
                }
            }),

            // 12: Forceclass the player to a random scp from the list Keywords: scp fc forceclass.
            new CoinFlipEffect(Translations.TurnIntoScpMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to forceclass a null or dead player to SCP."))
                        return;

                    player.DropItems();
                    player.Scale = Vector3.one;  // Reset scale to (1,1,1)

                    var randomScp = Config.ValidScps.ToList().RandomItem();
                    player.Role.Set(randomScp, RoleSpawnFlags.AssignInventory);
                
                    // Prevents the player from staying in PD forever.
                    if (player.CurrentRoom.Type == RoomType.Pocket)
                        player.EnableEffect(EffectType.PocketCorroding);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while forceclassing player to SCP: {ex.Message}");
                }
            }),
            
            // 13: Resets player's inventory.
            new CoinFlipEffect(Translations.InventoryResetMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to reset inventory for a null or dead player."))
                        return;

                    player.DropHeldItem();
                    player.ClearInventory();
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while resetting player's inventory: {ex.Message}");
                }
            }),

            // 14: Flips the players role to the opposite.
            new CoinFlipEffect(Translations.ClassSwapMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to flip role for a null or dead player."))
                        return;

                    player.DropItems();

                    player.Role.Set(player.Role.Type switch
                    {
                        RoleTypeId.Scientist => RoleTypeId.ClassD,
                        RoleTypeId.ClassD => RoleTypeId.Scientist,
                        RoleTypeId.ChaosConscript or RoleTypeId.ChaosRifleman => RoleTypeId.NtfSergeant,
                        RoleTypeId.ChaosMarauder or RoleTypeId.ChaosRepressor => RoleTypeId.NtfCaptain,
                        RoleTypeId.FacilityGuard => RoleTypeId.ChaosRifleman,
                        RoleTypeId.NtfPrivate or RoleTypeId.NtfSergeant or RoleTypeId.NtfSpecialist => RoleTypeId.ChaosRifleman,
                        RoleTypeId.NtfCaptain => new[] { RoleTypeId.ChaosMarauder, RoleTypeId.ChaosRepressor }.RandomItem(),
                        _ => player.Role.Type
                    }, RoleSpawnFlags.AssignInventory);

                    // Prevents the player from staying in Pocket Dimension forever.
                    if (player.CurrentRoom.Type == RoomType.Pocket)
                    {
                        player.EnableEffect(EffectType.PocketCorroding);
                    }
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while flipping player's role: {ex.Message}");
                }
            }),

            // 15: Spawns an HE grenade with a very short fuse time.
            new CoinFlipEffect(Translations.InstantExplosionMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to spawn instant explosion for a null or dead player."))
                        return;

                    ExplosiveGrenade instaBoom = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                    instaBoom.FuseTime = 0.1f;
                    instaBoom.SpawnActive(player.Position, player);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while spawning instant explosion: {ex.Message}");
                }
            }),

            // 16: Swaps positions with another random player.
            new CoinFlipEffect(Player.List.Count(x => x.IsAlive && !Config.PlayerSwapIgnoredRoles.Contains(x.Role.Type)) <= 1 ? Translations.PlayerSwapIfOneAliveMessage : Translations.PlayerSwapMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to swap positions for a null or dead player."))
                        return;

                    var playerList = Player.List
                    .Where(x => x.IsAlive && !Config.PlayerSwapIgnoredRoles.Contains(x.Role.Type) && x != player)
                    .ToList();

                    playerList.Remove(player);

                    if (!playerList.Any())
                        return;

                    var targetPlayer = playerList.RandomItem();
                    var pos = targetPlayer.Position;

                    targetPlayer.Teleport(player.Position);
                    player.Teleport(pos);

                    EventHandlers.SendBroadcast(targetPlayer, Translations.PlayerSwapMessage);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while swapping player positions: {ex.Message}");
                }
            }),

            // 17: Kicks the player.
            new CoinFlipEffect(Translations.KickMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to kick a null or dead player."))
                        return;

                    // Delay so the broadcast can be sent to the player and doesn't throw NRE.
                    Timing.CallDelayed(1f, () => player.Kick(Config.KickReason));
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while kicking player: {ex.Message}");
                }
            }),

            // 18: Swap with a spectator.
            new CoinFlipEffect(Player.List.Where(x => x.Role.Type == RoleTypeId.Spectator).IsEmpty() ? Translations.SpectSwapNoSpectsMessage : Translations.SpectSwapPlayerMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to swap with spectator for a null or dead player."))
                        return;

                    var spectList = Player.List.Where(x => x.Role.Type == RoleTypeId.Spectator).ToList();

                    if (!spectList.Any())
                        return;

                    var spect = spectList.RandomItem();

                    spect.Role.Set(player.Role.Type, RoleSpawnFlags.None);
                    spect.Teleport(player);
                    spect.Health = player.Health;

                    // Copy items to spectator.
                    foreach (var itemType in player.Items.Select(item => item.Type))
                    {
                        spect.AddItem(itemType);
                    }
                
                    // Give spect the player's ammo before clearing inventory.
                    foreach (var ammoEntry in player.Ammo)
                    {
                        var ammoType = ammoEntry.Key.GetAmmoType();
                        spect.AddAmmo(ammoType, ammoEntry.Value);
                        player.SetAmmo(ammoType, 0);
                    }

                    player.ClearInventory();
                    player.Role.Set(RoleTypeId.Spectator);

                    EventHandlers.SendBroadcast(spect, Translations.SpectSwapSpectMessage);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while swapping with spectator: {ex.Message}");
                }
            }),

            // 19: Teleports to a random Tesla gate if warhead is not detonated.
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.TeslaTpAfterWarheadMessage : Translations.TeslaTpMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to teleport to Tesla gate for a null or dead player."))
                        return;

                    player.DropHeldItem();

                    player.Teleport(Exiled.API.Features.TeslaGate.List.ToList().RandomItem());

                    if (Warhead.IsDetonated)
                    {
                        player.Kill(DamageType.Decontamination);
                    }
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while teleporting to Tesla gate: {ex.Message}");
                }
            }),

            // 20: Swaps inventory and ammo with another random player.
            new CoinFlipEffect(Player.List.Where(x => !Config.InventorySwapIgnoredRoles.Contains(x.Role.Type)).Count(x => x.IsAlive) <= 1 ? Translations.InventorySwapOnePlayerMessage : Translations.InventorySwapMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to swap inventory for a null or dead player."))
                        return;

                    var alivePlayers = Player.List
                        .Where(x => x != player && x.IsAlive && !Config.InventorySwapIgnoredRoles.Contains(x.Role.Type))
                        .ToList();

                    if (!alivePlayers.Any())
                    {
                        player.Hurt(25);
                        return;
                    }

                    var target = alivePlayers.RandomItem();

                    // Saving items.
                    var items1 = player.Items.Select(item => item.Type).ToList();
                    var items2 = target.Items.Select(item => item.Type).ToList();

                    var ammo1 = player.Ammo.ToDictionary(entry => entry.Key.GetAmmoType(), entry => entry.Value);
                    var ammo2 = target.Ammo.ToDictionary(entry => entry.Key.GetAmmoType(), entry => entry.Value);

                    // Saving and removing ammo.
                    player.ClearInventory();
                    target.ClearInventory();

                    foreach (var ammoType in ammo1.Keys)
                        player.SetAmmo(ammoType, 0);
                    foreach (var ammoType in ammo2.Keys)
                        target.SetAmmo(ammoType, 0);

                    // setting items
                    foreach (var itemType in items2)
                        player.AddItem(itemType);
                    foreach (var itemType in items1)
                        target.AddItem(itemType);

                    // setting ammo
                    foreach (var ammo in ammo2)
                        player.SetAmmo(ammo.Key, ammo.Value);
                    foreach (var ammo in ammo1)
                        target.SetAmmo(ammo.Key, ammo.Value);

                    EventHandlers.SendBroadcast(target, Translations.InventorySwapMessage);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while swapping inventory: {ex.Message}");
                }
            }),

            // 21: Spawns a red candy or teleports the player to a random room based on warhead state.
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.RandomTeleportWarheadDetonatedMessage : Translations.RandomTeleportMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to teleport or spawn candy for a null or dead player."))
                        return;

                    if (Warhead.IsDetonated)
                    {
                        CreateAndAddItem<Scp330>(player, ItemType.SCP330, candy =>
                            candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Red));
                        return;
                    }

                    player.Teleport(Room.Get(Config.RoomsToTeleport.GetRandomValue()));
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while teleporting or spawning candy: {ex.Message}");
                }
            }),

            // 22: Handcuffs the player and drops their items.
            new CoinFlipEffect(Translations.HandcuffMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to handcuff a null or dead player."))
                        return;

                    player.Handcuff();
                    player.DropItems();

                    Timing.CallDelayed(15f, () => {
                        if (player?.IsAlive == true)
                            player.RemoveHandcuffs();
                    });
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while handcuffing player: {ex.Message}");
                }
            }),

            // 23: Teleports all alive players (excluding Spectators and SCP-079) to their initial spawn location.
            new CoinFlipEffect(Translations.TeleportToSpawnMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to initiate spawn teleport with a null or dead player."))
                        return;

                    foreach (var p in Player.List.Where(x =>
                        x.IsAlive &&
                        x.Role.Type != RoleTypeId.Spectator &&
                        x.Role.Type != RoleTypeId.Scp079))
                    {
                        if (EventHandlers.InitialSpawnPositions.TryGetValue(p.UserId, out Vector3 spawnPos))
                        {
                            p.Teleport(spawnPos);
                            EventHandlers.SendBroadcast(p, Translations.TeleportToSpawnPlayerMessage);
                        }
                        else
                        {
                            Log.Warn($"No initial spawn position found for {p.Nickname}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while teleporting to spawn: {ex.Message}");
                }
            }),

            // 24: Broadcasts a fake NTF spawn message.
            new CoinFlipEffect(Translations.FakeNtfMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to broadcast fake NTF message for a null or dead player."))
                        return;

                    int scpCount = Player.Get(Side.Scp).Count();
                    var natoDesignations = new Dictionary<string, string>
                    {
                        { "November", "NATO_N" },
                        { "Bravo", "NATO_B" },
                        { "Charlie", "NATO_C" },
                        { "Delta", "NATO_D" },
                        { "Echo", "NATO_E" },
                        { "India", "NATO_I" },
                        { "Juliett", "NATO_J" },
                        { "Papa", "NATO_P" }
                    };
                    var randomNato = natoDesignations.ElementAt(Rd.Next(natoDesignations.Count));
                    string natoName = randomNato.Key;
                    string natoCode = randomNato.Value;
                    int randomNumber = Rd.Next(3, 18);

                    string message;
                    string displayMessage;

                    if (scpCount > 0)
                    {
                        message = $"mtfunit epsilon 11 designated {natoCode} {randomNumber} hasentered. allremaining awaitingrecontainment. {scpCount} scpsubject";
                        displayMessage = $"Mobile Task Force Unit, Epsilon-11, designated {natoName}-{randomNumber} has entered the facility. All remaining personnel are advised to proceed with standard evacuation protocols until an MTF squad reaches your destination. Awaiting re-containment of: {scpCount} SCP subject(s).";
                    }
                    else
                    {
                        message = $"mtfunit epsilon 11 designated {natoCode} {randomNumber} hasentered. allremaining noscpsleft";
                        displayMessage = $"Mobile Task Force Unit, Epsilon-11, designated {natoName}-{randomNumber}, has entered the facility. All remaining personnel are advised to proceed with standard evacuation protocols until an MTF squad reaches your destination. Substantial threat to safety remains within the facility -- exercise caution.";
                    }

                    Cassie.MessageTranslated(message, displayMessage);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while broadcasting fake NTF message: {ex.Message}");
                }
            }),

            // 25: Applies decontamination effect to all players in the Light zone for 5 seconds.
            new CoinFlipEffect(Translations.LightZoneDecontaminationMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to add decontamination effect to all players in light for null or dead players."))
                        return;

                    foreach (var p in Player.List.Where(x => x.CurrentRoom.Zone == ZoneType.LightContainment))
                    {
                        p.EnableEffect(EffectType.Decontaminating, 5, true);
                    }
                    Map.Broadcast(5, Translations.LightZoneDecontaminationMessage);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while applying decontamination effect: {ex.Message}");
                }
            }),

            // 26: Randomly teleports the player throughout the facility every 5 seconds for 20 seconds.
            new CoinFlipEffect(Translations.RandomTeleportationMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to randomly teleport a null or dead player."))
                        return;

                    const int teleportInterval = 5; // seconds.
                    const int totalTeleports = 4; // total number of teleports.
                    
                    void ScheduleTeleport(int remainingTeleports)
                    {
                        if (remainingTeleports <= 0 || player?.IsAlive != true)
                            return;

                        var randomRoom = Room.Get(Config.RoomsToTeleport.GetRandomValue());
                        if (randomRoom != null)
                        {
                            player.Teleport(randomRoom);
                            Timing.CallDelayed(teleportInterval, () => ScheduleTeleport(remainingTeleports - 1));
                        }
                    }

                    // Start the teleportation process.
                    ScheduleTeleport(totalTeleports);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while randomly teleporting player: {ex.Message}");
                }
            }),

            // 27: Turns the player upside down for 30 seconds.
            new CoinFlipEffect(Translations.UpsideDownScaleMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to turn upside down a null or dead player."))
                        return;

                    var originalScale = player.Scale;
                    player.Scale = new Vector3(1, -1, 1);

                    Timing.CallDelayed(30f, () => {
                        if (player?.IsAlive == true)
                            player.Scale = originalScale;
                    });
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while turning player upside down: {ex.Message}");
                }
            }),

            // 28: Locks all doors in the current zone.
            new CoinFlipEffect(Translations.ZoneDoorLockMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to lock doors in zone for a null or dead player."))
                        return;

                    var currentZone = player.CurrentRoom.Zone;
                    var doorsInZone = Door.List.Where(door => door.Room.Zone == currentZone).ToList();
                    var playerInZone = Player.List.Where(p => p.CurrentRoom.Zone == currentZone).ToList();

                    foreach (var door in doorsInZone)
                    {
                        door.IsOpen = false;
                        door.Lock(10f, DoorLockType.Regular079);
                    }

                    foreach (var affectedPlayer in playerInZone)
                    {
                        EventHandlers.SendBroadcast(affectedPlayer, Translations.ZoneDoorLockMessage);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while locking doors in zone: {ex.Message}");
                }
            }),

            // 29: Random item dropping effect.
            new CoinFlipEffect(Translations.RandomItemDropMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to start random item dropping for null or dead player."))
                        return;

                    void DropRandomItem()
                    {
                        if (player?.IsAlive == true && player.Items.Any())
                        {
                            var randomItem = player.Items.ToList().RandomItem();
                            player.DropItem(randomItem);
                            Timing.CallDelayed(120f, DropRandomItem);
                        }
                    }

                    DropRandomItem();
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while dropping random items: {ex.Message}");
                }
            }),

            // 30: Walking time bomb.
            new CoinFlipEffect(Translations.WalkingTimeBombMessage, player =>
            {
                try
                {
                    if (IsPlayerInvalid(player, "Attempted to set walking time bomb for null or dead player."))
                        return;

                    string PlayerUserId = player.UserId;
                    RoleTypeId originalRole = player.Role.Type;
                    float randomDelay = Rd.Next(10, 181); // Random time between 10-180 seconds.

                    Timing.CallDelayed(randomDelay, () =>
                    {
                        Player currentPlayer = Player.Get(PlayerUserId);
                        if (currentPlayer?.IsAlive == true && currentPlayer.Role.Type == originalRole)
                        {
                            ExplosiveGrenade instaBoom = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                            instaBoom.FuseTime = 0.1f;
                            instaBoom.SpawnActive(currentPlayer.Position, currentPlayer);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while giving walking bomb to player : {ex.Message}");
                }
            }),
        };
    }
}