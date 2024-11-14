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
    public class CoinFlipEffect
    {
        private static Config Config => Plugin.Instance.Config;
        private static Configs.Translations Translations => Plugin.Instance.Translation;
        private static readonly System.Random Rd = new();
        
        public Action<Player> Execute { get; set; }
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
            }),

            // 1: Spawns a medkit and painkillers for the player.
            new CoinFlipEffect(Translations.MediKitMessage, player =>
            {
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
            }),

            // 2: Teleports the player to the escape primary door.
            new CoinFlipEffect(Translations.TpToEscapeMessage, player =>
            {
                player.Teleport(Door.Get(DoorType.EscapePrimary));
            }),

            // 3: Heals the player by 25 health points.
            new CoinFlipEffect(Translations.MagicHealMessage, player =>
            {
                player.Health = player.MaxHealth;
            }),

            // 4: Increases the player's health by 10%.
            new CoinFlipEffect(Translations.HealthIncreaseMessage, player =>
            {
                player.Health *= 1.1f;
            }),

            // 5: Spawns random SCP item for the player.
            new CoinFlipEffect(Translations.RandomsScpItem, player =>
            {
                ItemType[] scpItems = { ItemType.SCP018, ItemType.SCP207, ItemType.AntiSCP207, ItemType.SCP268, ItemType.SCP500, ItemType.SCP330, ItemType.SCP1853, ItemType.SCP1576, ItemType.SCP244a, ItemType.SCP244b };
                ItemType randomScpItem = scpItems[Rd.Next(scpItems.Length)];

                if (player.Items.Count < 8)
                    player.AddItem(randomScpItem);
                else
                    Pickup.CreateAndSpawn(randomScpItem, player.Position, new Quaternion());
            }),

            // 6: Applies a random good effect to the player.
            new CoinFlipEffect(Translations.RandomGoodEffectMessage, player =>
            {
                var effect = Config.GoodEffects.ToList().RandomItem();
                player.EnableEffect(effect, 5, true);
                Log.Debug($"Chosen random effect: {effect}");
            }),

            // 7: Spawns a Logicer with one ammo for the player.
            new CoinFlipEffect(Translations.OneAmmoLogicerMessage, player =>
            {
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
            }),

            // 8: Spawns pink candy (SCP-330) for the player.
            new CoinFlipEffect(Translations.PinkCandyMessage, player =>
            {
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
            }),

            // 9: Spawns a customized revolver with attachments for the player.
            new CoinFlipEffect(Translations.BadRevoMessage, player =>
            {
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
            }),

            // 10: Spawns a MicroHID for the player.
            new CoinFlipEffect(Translations.SpawnHidMessage, player =>
            {
                MicroHIDPickup item = (MicroHIDPickup)Pickup.Create(ItemType.MicroHID);
                item.Position = player.Position;
                item.Spawn();
                item.Energy = 100;
            }),

            // 11: Forces a respawn wave of the team that has more ticketes
            new CoinFlipEffect(Translations.ForceRespawnMessage, player =>
            {
                Respawn.ForceWave(Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox ? SpawnableTeamType.NineTailedFox : SpawnableTeamType.ChaosInsurgency, true);
            }),

            // 12: Changes the player's size
            new CoinFlipEffect(Translations.SizeChangeMessage, player =>
            {
                player.Scale = new Vector3(1.13f, 0.5f, 1.13f);
                // Reset respawn count when size change effect is applied
                if (!EventHandlers.RespawnCount.ContainsKey(player.UserId))
                {
                    EventHandlers.RespawnCount[player.UserId] = 0;
                }
            }),

            // 13: Spawns a random item for the player.
            new CoinFlipEffect(Translations.RandomItemMessage, player =>
            {
                var randomItem = Config.ItemsToGive.ToList().RandomItem();
                if (player.Items.Count() < 8)
                    player.AddItem(randomItem);
                else
                    Item.Create(Config.ItemsToGive.ToList().RandomItem()).CreatePickup(player.Position);
            }),

            // 14: Refills all ammo and charges MicroHID
            new CoinFlipEffect(Translations.AmmoRefillMessage, player =>
            {
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
            }),

            // 15: Gives temporary godmode
            new CoinFlipEffect(Translations.TemporaryGodmodeMessage, player =>
            {
                player.IsGodModeEnabled = true;
                Timing.CallDelayed(5f, () => player.IsGodModeEnabled = false);
            }),

            // 16: upgrade keycard in inventory
            new CoinFlipEffect(Translations.KeycardUpgradedMessage, player =>
            {
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
            }),
        };

        // BadEffects list
        public static List<CoinFlipEffect> BadEffects = new()
        {
            // 0: Reduces player's health by 30%
            new CoinFlipEffect(Translations.HpReductionMessage, player =>
            {
                if ((int) player.Health == 1)
                    player.Kill(DamageType.CardiacArrest);
                else
                    player.Health *= 0.7f;
            }),

            // 1: Teleports the player to the class D cells.
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.TpToClassDCellsAfterWarheadMessage : Translations.TpToClassDCellsMessage, player =>
            {
                player.DropHeldItem();
                player.Teleport(Door.Get(DoorType.PrisonDoor));

                if (Warhead.IsDetonated)
                {
                    player.Kill(DamageType.Decontamination);
                }
            }),

            // 2: Applies a random bad effect to the player.
            new CoinFlipEffect(Translations.RandomBadEffectMessage, player =>
            {
                var effect = Config.BadEffects.ToList().RandomItem();
                
                //prevents players from staying in PD infinitely
                if (effect == EffectType.PocketCorroding)
                    player.EnableEffect(EffectType.PocketCorroding);
                else
                    player.EnableEffect(effect, 5, true);

                Log.Debug($"Chosen random effect: {effect}");
            }),

            // 3: Starts or stops the warhead based on its state.
            new CoinFlipEffect(Warhead.IsDetonated || !Warhead.IsInProgress ? Translations.WarheadStartMessage : Translations.WarheadStopMessage, player =>
            {
                if (Warhead.IsDetonated || !Warhead.IsInProgress)
                    Warhead.Start();
                else
                    Warhead.Stop();
            }),

            // 4: Turns off all lights
            new CoinFlipEffect(Translations.LightsOutMessage, player =>
            {
                Map.TurnOffAllLights(Config.MapBlackoutTime);
            }),

            // 5: Spawns a live HE grenade
            new CoinFlipEffect(Translations.LiveGrenadeMessage, player =>
            {
                ExplosiveGrenade grenade = (ExplosiveGrenade) Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = (float) Config.LiveGrenadeFuseTime;
                grenade.SpawnActive(player.Position + Vector3.up, player);
            }),

            // 6: Spawns a flash grenade with a short fuse time, sets the flash owner to the player so that it hopefully blinds people
            new CoinFlipEffect(Translations.TrollFlashMessage, player =>
            {
                FlashGrenade flash = (FlashGrenade) Item.Create(ItemType.GrenadeFlash, player);
                flash.FuseTime = 1f;
                flash.SpawnActive(player.Position);
            }),

            // 7: Teleports the player to a random SCP or inflicts damage if no SCPs exist.
            new CoinFlipEffect(Player.Get(Side.Scp).Any(x => x.Role.Type != RoleTypeId.Scp079) ? Translations.TpToRandomScpMessage : Translations.SmallDamageMessage, player =>
            {
                if (Player.Get(Side.Scp).Any(x => x.Role.Type != RoleTypeId.Scp079))
                {
                    Player scpPlayer = Player.Get(Side.Scp).Where(x => x.Role.Type != RoleTypeId.Scp079).ToList().RandomItem();
                    player.Position = scpPlayer.Position;
                    return;
                }
                player.Hurt(15);
            }),

            // 8: Sets player hp to 1 or kills if it was already 1
            new CoinFlipEffect(Translations.HugeDamageMessage, player =>
            {
                if ((int) player.Health == 1)
                    player.Kill(DamageType.CardiacArrest);
                else
                    player.Health = 1;
            }),

            // 9: Spawns a primed SCP-244 vase for the player.
            new CoinFlipEffect(Translations.PrimedVaseMessage, player =>
            {
                Scp244 vase = (Scp244)Item.Create(ItemType.SCP244a);
                vase.Primed = true;
                vase.CreatePickup(player.Position);
            }),

            // 10: Spawns a tantrum on the player Keywords: shit spawn create
            new CoinFlipEffect(Translations.ShitPantsMessage, player =>
            {
                player.PlaceTantrum();
            }),

            // 11: Broadcasts a fake SCP termination message.
            new CoinFlipEffect(Translations.FakeScpKillMessage, player =>
            {
                var scpName = _scpNames.ToList().RandomItem();

                Cassie.MessageTranslated($"scp {scpName.Key} successfully terminated by automatic security system",
                    $"{scpName.Value} successfully terminated by Automatic Security System.");
            }),

            // 12: Forceclass the player to a random scp from the list Keywords: scp fc forceclass
            new CoinFlipEffect(Translations.TurnIntoScpMessage, player =>
            {
                player.DropItems();
                player.Scale = new Vector3(1, 1, 1);

                var randomScp = Config.ValidScps.ToList().RandomItem();
                player.Role.Set(randomScp, RoleSpawnFlags.AssignInventory);
                
                //prevents the player from staying in PD forever
                if (player.CurrentRoom.Type == RoomType.Pocket)
                    player.EnableEffect(EffectType.PocketCorroding);
            }),
            
            // 13: Resets player's inventory
            new CoinFlipEffect(Translations.InventoryResetMessage, player =>
            {
                player.DropHeldItem();
                player.ClearInventory();
            }),

            // 14: Flips the players role to the opposite
            new CoinFlipEffect(Translations.ClassSwapMessage, player =>
            {
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
            }),

            // 15: Spawns an HE grenade with a very short fuse time
            new CoinFlipEffect(Translations.InstantExplosionMessage, player =>
            {
                ExplosiveGrenade instaBoom = (ExplosiveGrenade) Item.Create(ItemType.GrenadeHE);
                instaBoom.FuseTime = 0.1f;
                instaBoom.SpawnActive(player.Position, player);
            }),

            // 16: Swaps positions with another random player
            new CoinFlipEffect(Player.List.Count(x => x.IsAlive && !Config.PlayerSwapIgnoredRoles.Contains(x.Role.Type)) <= 1 ? Translations.PlayerSwapIfOneAliveMessage : Translations.PlayerSwapMessage, player =>
            {
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
            }),

            // 17: kicks the player
            new CoinFlipEffect(Translations.KickMessage, player =>
            {
                //delay so the broadcast can be sent to the player and doesn't throw NRE
                Timing.CallDelayed(1f, () => player.Kick(Config.KickReason));
            }),

            // 18: swap with a spectator
            new CoinFlipEffect(Player.List.Where(x => x.Role.Type == RoleTypeId.Spectator).IsEmpty() ? Translations.SpectSwapNoSpectsMessage : Translations.SpectSwapPlayerMessage, player =>
            {
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
            }),

            // 19: Teleports to a random Tesla gate if warhead is not detonated
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.TeslaTpAfterWarheadMessage : Translations.TeslaTpMessage, player =>
            {
                player.DropHeldItem();
                
                player.Teleport(Exiled.API.Features.TeslaGate.List.ToList().RandomItem());
                
                if (Warhead.IsDetonated)
                {
                    player.Kill(DamageType.Decontamination);
                }
            }),

            // 20: Swaps inventory and ammo with another random player
            new CoinFlipEffect(Player.List.Where(x => !Config.InventorySwapIgnoredRoles.Contains(x.Role.Type)).Count(x => x.IsAlive) <= 1 ? Translations.InventorySwapOnePlayerMessage : Translations.InventorySwapMessage, player =>
            {
                List<Player> playerList = Player.List.Where(x => x != player && !Config.InventorySwapIgnoredRoles.Contains(x.Role.Type)).ToList();
                
                if (playerList.Count(x => x.IsAlive) <= 1)
                {
                    player.Hurt(50);
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
            }),

            // 21: Spawns a red candy or teleports the player to a random room based on warhead state.
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.RandomTeleportWarheadDetonatedMessage : Translations.RandomTeleportMessage, player =>
            {
                if (Warhead.IsDetonated)
                {
                    Scp330 candy = (Scp330)Item.Create(ItemType.SCP330);
                    candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Red);
                    candy.CreatePickup(player.Position);
                    return;
                }
                
                player.Teleport(Room.Get(Config.RoomsToTeleport.GetRandomValue()));
            }),

            // 22: Handcuffs the player and drops their items
            new CoinFlipEffect(Translations.HandcuffMessage, player =>
            {
                player.Handcuff();
                player.DropItems();
                Timing.CallDelayed(15f, () => player.RemoveHandcuffs());
            }),

            // 23: Teleports everyone to their initial spawn location
            new CoinFlipEffect(Translations.TeleportToSpawnMessage, player =>
            {
                foreach (var p in Player.List)
                {
                    if (p.Role.Type == RoleTypeId.Scp079) continue;

                    if (EventHandlers.InitialSpawnPositions.TryGetValue(p.UserId, out Vector3 spawnPos))
                    {
                        p.Teleport(spawnPos);
                    }
                }
            }),

            // 24: Broadcasts a fake NTF spawn message
            new CoinFlipEffect(Translations.FakeNtfMessage, player =>
            {
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
                    message = $"mtfunit epsilon 11 designated {natoCode} {randomNumber} hasentered allremaining awaitingrecontainment {scpCount} scpsubject";
                    displayMessage = $"Mobile Task Force Unit, Epsilon-11, designated {natoName}-{randomNumber} has entered the facility. All remaining personnel are advised to proceed with standard evacuation protocols until an MTF squad reaches your destination. Awaiting re-containment of: {scpCount} SCP subject(s).";
                }
                else
                {
                    message = $"mtfunit epsilon 11 designated {natoCode} {randomNumber} hasentered allremaining noscpsleft";
                    displayMessage = $"Mobile Task Force Unit, Epsilon-11, designated {natoName}-{randomNumber} has entered the facility. All remaining personnel are advised to proceed with standard evacuation protocols until an MTF squad reaches your destination. Substantial threat to safety remains within the facility -- exercise caution.";
                }

                Cassie.MessageTranslated(message, displayMessage);
            }),

            // 25: Applies decontamination effect to all players in the Light zone for 5 seconds.
            new CoinFlipEffect(Translations.LightZoneDecontaminationMessage, player =>
            {
                foreach (var p in Player.List)
                {
                    if (p.CurrentRoom.Zone == ZoneType.LightContainment)
                    {
                        p.EnableEffect(EffectType.Decontaminating, 5, true);
                    }
                }
                Map.Broadcast(5, Translations.LightZoneDecontaminationMessage);
            }),

            // 26: Randomly teleports the player throughout the facility every 5 seconds for 20 seconds.
            new CoinFlipEffect(Translations.RandomTeleportationMessage, player =>
            {
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
            }),
        };
    }
}