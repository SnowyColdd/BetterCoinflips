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

        // GoodEffects list
        public static List<CoinFlipEffect> GoodEffects = new()
        {
            // 0: Gives player a random card
            new CoinFlipEffect(Translations.RandomCardMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to give a random keycard to a null or dead player");
                    }

                    ItemType[] keycards = {
                    ItemType.KeycardJanitor, ItemType.KeycardScientist, ItemType.KeycardResearchCoordinator,
                    ItemType.KeycardFacilityManager, ItemType.KeycardGuard, ItemType.KeycardMTFOperative,
                    ItemType.KeycardMTFCaptain, ItemType.KeycardContainmentEngineer, ItemType.KeycardChaosInsurgency,
                    ItemType.KeycardZoneManager, ItemType.KeycardMTFPrivate, ItemType.KeycardO5 };
                    ItemType randomKeycard = keycards[Rd.Next(keycards.Length)];

                    if (player.Items.Count() < 8)
                        player.AddItem(randomKeycard);
                    else
                        Pickup.CreateAndSpawn(randomKeycard, player.Position, new Quaternion());
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn medkit and painkillers for a null or dead player.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to teleport a null or dead player to escape.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to heal a null or dead player.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to increase health of a null or dead player.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn random SCP item for a null or dead player.");
                        return;
                    }

                    ItemType[] scpItems = { ItemType.SCP018, ItemType.SCP207, ItemType.AntiSCP207, ItemType.SCP268, ItemType.SCP500, ItemType.SCP330, ItemType.SCP1853, ItemType.SCP1576, ItemType.SCP244a, ItemType.SCP244b };
                    ItemType randomScpItem = scpItems[Rd.Next(scpItems.Length)];

                    if (player.Items.Count < 8)
                        player.AddItem(randomScpItem);
                    else
                        Pickup.CreateAndSpawn(randomScpItem, player.Position, new Quaternion());
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to apply a random good effect to a null or dead player.");
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

            // 7: Spawns a Logicer with one ammo for the player.
            new CoinFlipEffect(Translations.OneAmmoLogicerMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn Logicer for a null or dead player.");
                        return;
                    }

                    if (player.Items.Count() < 8)
                    {
                        Firearm gun = (Firearm)Item.Create(ItemType.GunLogicer);
                        gun.Ammo = 1;
                        player.AddItem(gun);
                    }
                    else
                    {
                        Firearm gun = (Firearm)Item.Create(ItemType.GunLogicer);
                        gun.Ammo = 1;
                        gun.CreatePickup(player.Position);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning Logicer: {ex.Message}");
                }
            }),

            // 8: Spawns pink candy (SCP-330) for the player.
            new CoinFlipEffect(Translations.PinkCandyMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn pink candy for a null or dead player.");
                        return;
                    }

                    if (player.Items.Count() < 8)
                    {
                        Scp330 candy = (Scp330)Item.Create(ItemType.SCP330);
                        candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);
                        player.AddItem(candy);
                    }
                    else
                    {
                        Scp330 candy = (Scp330)Item.Create(ItemType.SCP330);
                        candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);
                        candy.CreatePickup(player.Position);
                    }
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn customized revolver for a null or dead player.");
                        return;
                    }

                    if (player.Items.Count() < 8)
                    {
                        Firearm revo = (Firearm)Item.Create(ItemType.GunRevolver);
                        revo.AddAttachment(new[]
                            {AttachmentName.CylinderMag8, AttachmentName.ShortBarrel, AttachmentName.ScopeSight});
                        player.AddItem(revo);
                    }
                    else
                    {
                        Firearm revo = (Firearm)Item.Create(ItemType.GunRevolver);
                        revo.AddAttachment(new[]
                            {AttachmentName.CylinderMag8, AttachmentName.ShortBarrel, AttachmentName.ScopeSight});
                        revo.CreatePickup(player.Position);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning customized revolver: {ex.Message}");
                }
            }),

            // 10: Spawns a MicroHID for the player.
            new CoinFlipEffect(Translations.SpawnHidMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn MicroHID for a null or dead player.");
                        return;
                    }

                    MicroHIDPickup item = (MicroHIDPickup)Pickup.Create(ItemType.MicroHID);
                    item.Position = player.Position;
                    item.Spawn();
                    item.Energy = 100;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning MicroHID: {ex.Message}");
                }
            }),

            // 11: Forces a respawn wave of the team that has more ticketes
            new CoinFlipEffect(Translations.ForceRespawnMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to force respawn wave for a null or dead player.");
                        return;
                    }

                    Respawn.ForceWave(Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox ? SpawnableTeamType.NineTailedFox : SpawnableTeamType.ChaosInsurgency, true);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while forcing respawn wave: {ex.Message}");
                }
            }),

            // 12: Changes the player's size
            new CoinFlipEffect(Translations.SizeChangeMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to change size of a null or dead player.");
                        return;
                    }

                    player.Scale = new Vector3(1.13f, 0.5f, 1.13f);
                    // Reset respawn count when size change effect is applied
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn random item for a null or dead player.");
                        return;
                    }

                    var randomItem = Config.ItemsToGive.ToList().RandomItem();
                    if (player.Items.Count() < 8)
                        player.AddItem(randomItem);
                    else
                        Item.Create(Config.ItemsToGive.ToList().RandomItem()).CreatePickup(player.Position);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while spawning random item: {ex.Message}");
                }
            }),

            // 14: Refills all ammo and charges MicroHID
            new CoinFlipEffect(Translations.AmmoRefillMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to refill ammo for a null or dead player.");
                        return;
                    }

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

                    var armors = player.Items.Count(x => x.Type == ItemType.ArmorLight || x.Type == ItemType.ArmorCombat || x.Type == ItemType.ArmorHeavy);
                    var ammoLimits = armors > 1 ? armorAmmoLimits[ItemType.None] : armorAmmoLimits[player.Items.FirstOrDefault(x => x.Type == ItemType.ArmorLight || x.Type == ItemType.ArmorCombat || x.Type == ItemType.ArmorHeavy)?.Type ?? ItemType.None];

                    foreach (var ammoLimit in ammoLimits)
                    {
                        player.SetAmmo(ammoLimit.Key, ammoLimit.Value);
                    }

                    foreach (var item in player.Items)
                    {
                        if (item is MicroHid microHid)
                        {
                            microHid.Energy = 100;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while refilling ammo: {ex.Message}");
                }
            }),

            // 15: Gives temporary godmode
            new CoinFlipEffect(Translations.TemporaryGodmodeMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to give temporary godmode to a null or dead player.");
                        return;
                    }

                    player.IsGodModeEnabled = true;
                    Timing.CallDelayed(5f, () => player.IsGodModeEnabled = false);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while giving temporary godmode: {ex.Message}");
                }
            }),

            // 16: Upgrade keycard in inventory
            new CoinFlipEffect(Translations.KeycardUpgradedMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to upgrade keycard for a null or dead player.");
                        return;
                    }

                    var keycards = player.Items.Where(item => item.Type.ToString().Contains("Keycard")).ToList();

                    if (!keycards.Any())
                    {
                        EventHandlers.SendBroadcast(player, Translations.NoKeycardMessage);
                        return;
                    }

                    var cardToUpgrade = keycards.Count == 1 ? keycards.First() : keycards[Rd.Next(keycards.Count)];

                    ItemType newCard = cardToUpgrade.Type switch
                    {
                        ItemType.KeycardJanitor => ItemType.KeycardScientist,
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to apply all positive effects to a null or dead player.");
                        return;
                    }

                    foreach (var effect in Config.GoodEffects)
                    {
                        player.EnableEffect(effect, 60, true);
                    }
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to give random HP to a null or dead player.");
                        return;
                    }

                    int randomHp = Rd.Next(1, 151);
                    player.Health = randomHp;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while giving random HP: {ex.Message}");
                }
            }),

            // 19: Grants the player 1000 HP for a configurable duration
            new CoinFlipEffect(Translations.ThousandHpMessage.Replace("{duration}", Config.ThousandHpDuration.ToString()), player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to grant 1000 HP to a null or dead player.");
                        return;
                    }

                    float originalHealth = player.Health;
                    player.Health = 1000;

                    // Revert HP back to original after the duration
                    Timing.CallDelayed(Config.ThousandHpDuration, () =>
                    {
                        if (player.IsAlive)
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to activate a random generator for a null or dead player.");
                        return;
                    }

                    // Logic to activate a random generator
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
        };

        // BadEffects list
        public static List<CoinFlipEffect> BadEffects = new()
        {
            // 0: Reduces player's health by 30%
            new CoinFlipEffect(Translations.HpReductionMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to reduce health of a null or dead player.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to teleport a null or dead player to class D cells.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to apply a random bad effect to a null or dead player.");
                        return;
                    }

                    var effect = Config.BadEffects.ToList().RandomItem();
                
                    //prevents players from staying in PD infinitely
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to toggle warhead for a null or dead player.");
                        return;
                    }

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

            // 4: Turns off all lights
            new CoinFlipEffect(Translations.LightsOutMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to turn off lights for a null or dead player.");
                        return;
                    }

                    Map.TurnOffAllLights(Config.MapBlackoutTime);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while turning off lights: {ex.Message}");
                }
            }),

            // 5: Spawns a live HE grenade
            new CoinFlipEffect(Translations.LiveGrenadeMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn live grenade for a null or dead player.");
                        return;
                    }

                    ExplosiveGrenade grenade = (ExplosiveGrenade) Item.Create(ItemType.GrenadeHE);
                    grenade.FuseTime = (float) Config.LiveGrenadeFuseTime;
                    grenade.SpawnActive(player.Position + Vector3.up, player);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while spawning live grenade: {ex.Message}");
                }
            }),

            // 6: Spawns a flash grenade with a short fuse time, sets the flash owner to the player so that it hopefully blinds people
            new CoinFlipEffect(Translations.TrollFlashMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn flash grenade for a null or dead player.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to teleport a null or dead player to random SCP.");
                        return;
                    }

                    if (Player.Get(Side.Scp).Any(x => x.Role.Type != RoleTypeId.Scp079))
                    {
                        Player scpPlayer = Player.Get(Side.Scp).Where(x => x.Role.Type != RoleTypeId.Scp079).ToList().RandomItem();
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

            // 8: Sets player hp to 1 or kills if it was already 1
            new CoinFlipEffect(Translations.HugeDamageMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to set HP to 1 for a null or dead player.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn primed SCP-244 vase for a null or dead player.");
                        return;
                    }

                    Scp244 vase = (Scp244)Item.Create(ItemType.SCP244a);
                    vase.Primed = true;
                    vase.CreatePickup(player.Position);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while spawning primed SCP-244 vase: {ex.Message}");
                }
            }),

            // 10: Spawns a tantrum on the player Keywords: shit spawn create
            new CoinFlipEffect(Translations.ShitPantsMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to place tantrum for a null or dead player.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to broadcast fake SCP termination message for a null or dead player.");
                        return;
                    }

                    var scpName = _scpNames.ToList().RandomItem();

                    Cassie.MessageTranslated($"scp {scpName.Key} successfully terminated by automatic security system",
                        $"{scpName.Value} successfully terminated by Automatic Security System.");
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while broadcasting fake SCP termination message: {ex.Message}");
                }
            }),

            // 12: Forceclass the player to a random scp from the list Keywords: scp fc forceclass
            new CoinFlipEffect(Translations.TurnIntoScpMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to forceclass a null or dead player to SCP.");
                        return;
                    }

                    player.DropItems();
                    player.Scale = new Vector3(1, 1, 1);

                    var randomScp = Config.ValidScps.ToList().RandomItem();
                    player.Role.Set(randomScp, RoleSpawnFlags.AssignInventory);
                
                    //prevents the player from staying in PD forever
                    if (player.CurrentRoom.Type == RoomType.Pocket)
                        player.EnableEffect(EffectType.PocketCorroding);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while forceclassing player to SCP: {ex.Message}");
                }
            }),
            
            // 13: Resets player's inventory
            new CoinFlipEffect(Translations.InventoryResetMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to reset inventory for a null or dead player.");
                        return;
                    }

                    player.DropHeldItem();
                    player.ClearInventory();
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while resetting player's inventory: {ex.Message}");
                }
            }),

            // 14: Flips the players role to the opposite
            new CoinFlipEffect(Translations.ClassSwapMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to flip role for a null or dead player.");
                        return;
                    }

                    player.DropItems();
                    switch (player.Role.Type)
                    {
                        case RoleTypeId.Scientist:
                            player.Role.Set(RoleTypeId.ClassD, RoleSpawnFlags.AssignInventory);
                            break;
                        case RoleTypeId.ClassD:
                            player.Role.Set(RoleTypeId.Scientist, RoleSpawnFlags.AssignInventory);
                            break;
                        case RoleTypeId.ChaosConscript:
                        case RoleTypeId.ChaosRifleman:
                            player.Role.Set(RoleTypeId.NtfSergeant, RoleSpawnFlags.AssignInventory);
                            break;
                        case RoleTypeId.ChaosMarauder:
                        case RoleTypeId.ChaosRepressor:
                            player.Role.Set(RoleTypeId.NtfCaptain, RoleSpawnFlags.AssignInventory);
                            break;
                        case RoleTypeId.FacilityGuard:
                            player.Role.Set(RoleTypeId.ChaosRifleman, RoleSpawnFlags.AssignInventory);
                            break;
                        case RoleTypeId.NtfPrivate:
                        case RoleTypeId.NtfSergeant:
                        case RoleTypeId.NtfSpecialist:
                            player.Role.Set(RoleTypeId.ChaosRifleman, RoleSpawnFlags.AssignInventory);
                            break;
                        case RoleTypeId.NtfCaptain:
                            List<RoleTypeId> roles = new List<RoleTypeId>
                            {
                                RoleTypeId.ChaosMarauder,
                                RoleTypeId.ChaosRepressor
                            };
                            player.Role.Set(roles.RandomItem(), RoleSpawnFlags.AssignInventory);
                            break;
                    }

                    //prevents the player from staying in PD forever
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

            // 15: Spawns an HE grenade with a very short fuse time
            new CoinFlipEffect(Translations.InstantExplosionMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to spawn instant explosion for a null or dead player.");
                        return;
                    }

                    ExplosiveGrenade instaBoom = (ExplosiveGrenade) Item.Create(ItemType.GrenadeHE);
                    instaBoom.FuseTime = 0.1f;
                    instaBoom.SpawnActive(player.Position, player);
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while spawning instant explosion: {ex.Message}");
                }
            }),

            // 16: Swaps positions with another random player
            new CoinFlipEffect(Player.List.Count(x => x.IsAlive && !Config.PlayerSwapIgnoredRoles.Contains(x.Role.Type)) <= 1 ? Translations.PlayerSwapIfOneAliveMessage : Translations.PlayerSwapMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to swap positions for a null or dead player.");
                        return;
                    }

                    var playerList = Player.List.Where(x => x.IsAlive && !Config.PlayerSwapIgnoredRoles.Contains(x.Role.Type)).ToList();
                    playerList.Remove(player);

                    if (playerList.IsEmpty())
                    {
                        return;
                    }

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

            // 17: Kicks the player
            new CoinFlipEffect(Translations.KickMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to kick a null or dead player.");
                        return;
                    }

                    //delay so the broadcast can be sent to the player and doesn't throw NRE
                    Timing.CallDelayed(1f, () => player.Kick(Config.KickReason));
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while kicking player: {ex.Message}");
                }
            }),

            // 18: Swap with a spectator
            new CoinFlipEffect(Player.List.Where(x => x.Role.Type == RoleTypeId.Spectator).IsEmpty() ? Translations.SpectSwapNoSpectsMessage : Translations.SpectSwapPlayerMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to swap with spectator for a null or dead player.");
                        return;
                    }

                    var spectList = Player.List.Where(x => x.Role.Type == RoleTypeId.Spectator).ToList();

                    if (spectList.IsEmpty())
                    {
                        return;
                    }

                    var spect = spectList.RandomItem();

                    spect.Role.Set(player.Role.Type, RoleSpawnFlags.None);
                    spect.Teleport(player);
                    spect.Health = player.Health;

                    List<ItemType> playerItems = player.Items.Select(item => item.Type).ToList();

                    foreach (var item in playerItems)
                    {
                        spect.AddItem(item);
                    }
                
                    //give spect the players ammo, has to be done before ClearInventory() or else ammo will fall on the floor
                    for (int i = 0; i < player.Ammo.Count; i++)
                    {
                        spect.AddAmmo(player.Ammo.ElementAt(i).Key.GetAmmoType(), player.Ammo.ElementAt(i).Value);
                        player.SetAmmo(player.Ammo.ElementAt(i).Key.GetAmmoType(), 0);
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

            // 19: Teleports to a random Tesla gate if warhead is not detonated
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.TeslaTpAfterWarheadMessage : Translations.TeslaTpMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to teleport to Tesla gate for a null or dead player.");
                        return;
                    }

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

            // 20: Swaps inventory and ammo with another random player
            new CoinFlipEffect(Player.List.Where(x => !Config.InventorySwapIgnoredRoles.Contains(x.Role.Type)).Count(x => x.IsAlive) <= 1 ? Translations.InventorySwapOnePlayerMessage : Translations.InventorySwapMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to swap inventory for a null or dead player.");
                        return;
                    }

                    List<Player> playerList = Player.List.Where(x => x != player && !Config.InventorySwapIgnoredRoles.Contains(x.Role.Type)).ToList();

                    if (playerList.Count(x => x.IsAlive) <= 1)
                    {
                        player.Hurt(25);
                        return;
                    }

                    var target = playerList.Where(x => x != player).ToList().RandomItem();

                    // Saving items
                    List<ItemType> items1 = player.Items.Select(item => item.Type).ToList();
                    List<ItemType> items2 = target.Items.Select(item => item.Type).ToList();

                    // Saving and removing ammo
                    Dictionary<AmmoType, ushort> ammo1 = new();
                    Dictionary<AmmoType, ushort> ammo2 = new();
                    for (int i = 0; i < player.Ammo.Count; i++)
                    {
                        ammo1.Add(player.Ammo.ElementAt(i).Key.GetAmmoType(), player.Ammo.ElementAt(i).Value);
                        player.SetAmmo(ammo1.ElementAt(i).Key, 0);
                    }
                    for (int i = 0; i < target.Ammo.Count; i++)
                    {
                        ammo2.Add(target.Ammo.ElementAt(i).Key.GetAmmoType(), target.Ammo.ElementAt(i).Value);
                        target.SetAmmo(ammo2.ElementAt(i).Key, 0);
                    }

                    // setting items
                    target.ResetInventory(items1);
                    player.ResetInventory(items2);

                    // setting ammo
                    foreach (var ammo in ammo2)
                    {
                        player.SetAmmo(ammo.Key, ammo.Value);
                    }
                    foreach (var ammo in ammo1)
                    {
                        target.SetAmmo(ammo.Key, ammo.Value);
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to teleport or spawn candy for a null or dead player.");
                        return;
                    }

                    if (Warhead.IsDetonated)
                    {
                        Scp330 candy = (Scp330)Item.Create(ItemType.SCP330);
                        candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Red);
                        candy.CreatePickup(player.Position);
                        return;
                    }

                    player.Teleport(Room.Get(Config.RoomsToTeleport.GetRandomValue()));
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while teleporting or spawning candy: {ex.Message}");
                }
            }),

            // 22: Handcuffs the player and drops their items
            new CoinFlipEffect(Translations.HandcuffMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to handcuff a null or dead player.");
                        return;
                    }

                    player.Handcuff();
                    player.DropItems();
                    Timing.CallDelayed(15f, () => player.RemoveHandcuffs());
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while handcuffing player: {ex.Message}");
                }
            }),

            // 23: Teleports everyone to their initial spawn location
            new CoinFlipEffect(Translations.TeleportToSpawnMessage, player =>
            {
                try
                {
                    foreach (var p in Player.List)
                    {
                        if (p.Role.Type == RoleTypeId.Scp079) continue;

                        if (EventHandlers.InitialSpawnPositions.TryGetValue(p.UserId, out Vector3 spawnPos))
                        {
                            p.Teleport(spawnPos);
                        }
                    }
                }
                catch ( Exception ex )
                {
                    Log.Error($"Error while teleporting to spawn: {ex.Message}");
                }
            }),

            // 24: Broadcasts a fake NTF spawn message
            new CoinFlipEffect(Translations.FakeNtfMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to broadcast fake NTF message for a null or dead player.");
                        return;
                    }

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
                        displayMessage = $"Mobile Task Force Unit, Epsilon-11, designated {natoName}-{randomNumber}, has entered the facility. All remaining personnel are advised to proceed with standard evacuation protocols until an MTF squad reaches your destination . Substantial threat to safety remains within the facility -- exercise caution.";
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
                    foreach (var p in Player.List)
                    {
                        if (p.CurrentRoom.Zone == ZoneType.LightContainment)
                        {
                            p.EnableEffect(EffectType.Decontaminating, 5, true);
                        }
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to randomly teleport a null or dead player.");
                        return;
                    }

                    int teleportsRemaining = 4;

                    void TeleportPlayer()
                    {
                        if (teleportsRemaining > 0 && player.IsAlive)
                        {
                            player.Teleport(Room.Get(Config.RoomsToTeleport.GetRandomValue()));
                            teleportsRemaining--;

                            if (teleportsRemaining > 0)
                            {
                                Timing.CallDelayed(5f, TeleportPlayer);
                            }
                        }
                    }

                    TeleportPlayer();
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while randomly teleporting player: {ex.Message}");
                }
            }),

            // 27: Turns the player upside down with noganmi for 30 seconds.
            new CoinFlipEffect(Translations.UpsideDownScaleMessage, player =>
            {
                try
                {
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to turn upside down a null or dead player.");
                        return;
                    }

                    var originalScale = player.Scale;
                    player.Scale = new Vector3(1, -1, 1);
                    Timing.CallDelayed(30f, () => player.Scale = originalScale);
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to lock doors in zone for a null or dead player.");
                        return;
                    }

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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to start random item dropping for null or dead player.");
                        return;
                    }

                    void DropRandomItem()
                    {
                        if (player.IsAlive && player.Items.Any())
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
                    if (player == null || !player.IsAlive)
                    {
                        Log.Warn("Attempted to start random item dropping for null or dead player.");
                        return;
                    }

                    string PlayerUserId = player.UserId;
                    Timing.CallDelayed(10f, () =>
                    {
                        Player currentPlayer = Player.Get(PlayerUserId);
                        if (currentPlayer != null && currentPlayer.IsAlive && currentPlayer.Role == player.Role)
                        {
                            ExplosiveGrenade instaBoom = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                            instaBoom.FuseTime = 0.1f;
                            instaBoom.SpawnActive(player.Position, player);
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