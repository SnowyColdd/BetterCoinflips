using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using System.Linq;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UnityEngine;

namespace BetterCoinflips
{

    public class EventHandlers
    {
        private readonly Config _cfg = Plugin.Instance.Config;
        private readonly Translations _tr = Plugin.Instance.Translation;
        private System.Random rd = new();
        private int test = 0;
        private readonly Dictionary<string, string> _scpNames = new()
        {
            { "1 7 3", "SCP-173"},
            { "9 3 9", "SCP-939"},
            { "0 9 6", "SCP-096"},
            { "0 7 9", "SCP-079"},
            { "0 4 9", "SCP-049"},
            { "1 0 6", "SCP-106"}
        };

        private void SendBroadcast(Player pl, string message)
        {
            pl.Broadcast(_cfg.BroadcastTime, message);
        }

        public void OnCoinFlip(FlippingCoinEventArgs ev)
        {
            Log.Debug($"Is tails: {ev.IsTails}");
            if (!ev.IsTails)
            {
                Dictionary<int, int> effectChances = new Dictionary<int, int>
                {
                    { 1, _cfg.KeycardEffectChance },
                    { 2, _cfg.MedicalKitEffectChance },
                    { 3, _cfg.TPToEscapeEffectChance },
                    { 4, _cfg.HealEffectChance },
                    { 5, _cfg.MoreHPEffectChance },
                    { 6, _cfg.HatEffectChance },
                    { 7, _cfg.RandomGoodEffectChance },
                    { 8, _cfg.OneAmmoLogicerEffectChance },
                    { 9, _cfg.LightbulbEffectChance },
                    { 10, _cfg.PinkCandyEffectChance}
                };
                int totalChance = effectChances.Values.Sum();
                int randomNum = rd.Next(1, totalChance + 1);
                int headsEvent = 2; // Set a default value for headsEvent

                foreach (KeyValuePair<int, int> kvp in effectChances)
                {
                    if (randomNum <= kvp.Value)
                    {
                        headsEvent = kvp.Key;
                        break;
                    }

                    randomNum -= kvp.Value;
                }

                Log.Debug($"headsEvent = {headsEvent}");

                switch (headsEvent) //TODO: Add effects: spawning 173 shit, 
                {
                    case 1:
                        if (_cfg.RedCardChance > rd.Next(1, 101))
                        {
                            Pickup.CreateAndSpawn(ItemType.KeycardFacilityManager, ev.Player.Position, new Quaternion());
                            SendBroadcast(ev.Player, _tr.RedCardMessage);
                        }
                        else
                        {
                            Pickup.CreateAndSpawn(ItemType.KeycardContainmentEngineer, ev.Player.Position, new Quaternion());
                            SendBroadcast(ev.Player, _tr.ContainmentEngineerCardMessage);
                        }

                        break;
                    case 2:
                        Pickup.CreateAndSpawn(ItemType.Medkit, ev.Player.Position, new Quaternion(0, 0, 0, 0));
                        Pickup.CreateAndSpawn(ItemType.Painkillers, ev.Player.Position, new Quaternion(0, 0, 0, 0));
                        SendBroadcast(ev.Player, _tr.MediKitMessage);
                        break;
                    case 3:
                        ev.Player.Teleport(Door.Get(DoorType.EscapeSecondary));
                        SendBroadcast(ev.Player, _tr.TpToEscapeMessage);
                        break;
                    case 4:
                        ev.Player.Heal(25);
                        SendBroadcast(ev.Player, _tr.MagicHealMessage);
                        break;
                    case 5:
                        ev.Player.Health *= 1.1f;
                        SendBroadcast(ev.Player, _tr.HealthIncreaseMessage);
                        break;
                    case 6:
                        Pickup.CreateAndSpawn(ItemType.SCP268, ev.Player.Position, new Quaternion(0, 0, 0, 0));
                        SendBroadcast(ev.Player, _tr.NeatHatMessage);
                        break;
                    case 7:
                        ev.Player.EnableEffect(_cfg.GoodEffects.ToList().RandomItem(), 5, true);
                        SendBroadcast(ev.Player, _tr.RandomGoodEffectMessage);
                        break;
                    case 8:
                        Item gun = Item.Create(ItemType.GunLogicer);
                        Firearm f = gun as Firearm;
                        f.Ammo = 1;
                        f.CreatePickup(ev.Player.Position);
                        SendBroadcast(ev.Player, _tr.OneAmmoLogicerMessage);
                        break;
                    case 9:
                        Pickup.CreateAndSpawn(ItemType.SCP2176, ev.Player.Position, new Quaternion(0, 0, 0, 0));
                        SendBroadcast(ev.Player, _tr.LightbulbMessage);
                        break;
                    case 10:
                        Scp330 candy = (Scp330)Item.Create(ItemType.SCP330);
                        candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);
                        candy.CreatePickup(ev.Player.Position);
                        SendBroadcast(ev.Player, _tr.PinkCandyMessage);
                        break;
                }
            }
            if (ev.IsTails)
            {
                Dictionary<int, int> effectChances = new Dictionary<int, int>
                {
                    { 1, _cfg.HpReductionEffectChance },
                    { 2, _cfg.TPToClassDCellsEffectChance },
                    { 3, _cfg.RandomBadEffectChance },
                    { 4, _cfg.WarheadEffectChance },
                    { 5, _cfg.LightsOutEffectChance },
                    { 6, _cfg.LiveHEEffectChance },
                    { 7, _cfg.TrollGunEffectChance },
                    { 8, _cfg.TrollFlashEffectChance },
                    { 9, _cfg.SCPTpEffectChance },
                    { 10, _cfg.OneHPLeftEffectChance },
                    { 11, _cfg.PrimedVaseEffectChance},
                    { 12, _cfg.ShitPantsEffectChance },
                    { 13, _cfg.FakeCassieEffectChance }
                };
                int totalChance = effectChances.Values.Sum();
                int randomNum = rd.Next(1, totalChance + 1);
                int tailsEvent = 13; // Set a default value for headsEvent

                foreach (KeyValuePair<int, int> kvp in effectChances)
                {
                    if (randomNum <= kvp.Value)
                    {
                        tailsEvent = kvp.Key;
                        break;
                    }

                    randomNum -= kvp.Value;
                }

                Log.Debug($"tailsEvent = {tailsEvent}");

                switch (tailsEvent)
                {
                    case 1:
                        ev.Player.Health *= 0.7f;
                        SendBroadcast(ev.Player, _tr.HPReductionMessage);
                        break;
                    case 2:
                        ev.Player.Teleport(Door.Get(DoorType.PrisonDoor));
                        SendBroadcast(ev.Player, _tr.TPToClassDCellsMessage);
                        break;
                    case 3:
                        ev.Player.EnableEffect(_cfg.BadEffects.ToList().RandomItem(), 5, true);
                        SendBroadcast(ev.Player, _tr.RandomBadEffectMessage);
                        break;
                    case 4:
                        if (!Warhead.IsDetonated)
                        {
                            if (Warhead.IsInProgress)
                            {
                                Warhead.Stop();
                                SendBroadcast(ev.Player, _tr.WarheadStopMessage);
                            }
                            else
                            {
                                Warhead.Start();
                                SendBroadcast(ev.Player, _tr.WarheadStartMessage);
                            }
                        }
                        else
                        {
                            Warhead.Start();
                            SendBroadcast(ev.Player, _tr.WarheadStartMessage);
                        }

                        break;
                    case 5:
                        Map.TurnOffAllLights(_cfg.MapBlackoutTime);
                        SendBroadcast(ev.Player, _tr.LightsOutMessage);
                        break;
                    case 6:
                        ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                        grenade.FuseTime = (float)_cfg.LiveGrenadeFuseTime;
                        grenade.SpawnActive(ev.Player.Position + Vector3.up);
                        SendBroadcast(ev.Player, _tr.LiveGrenadeMessage);
                        break;
                    case 7:
                        Item gun2 = Item.Create(ItemType.ParticleDisruptor);
                        Firearm f2 = gun2 as Firearm;
                        f2.Ammo = 0;
                        f2.CreatePickup(ev.Player.Position);
                        SendBroadcast(ev.Player, _tr.TrollGunMessage);
                        break;
                    case 8:
                        FlashGrenade flash = (FlashGrenade)Item.Create(ItemType.GrenadeFlash);
                        flash.FuseTime = 1f;
                        flash.SpawnActive(ev.Player.Position);
                        SendBroadcast(ev.Player, _tr.TrollFlashMessage);
                        break;
                    case 9:
                        if (Player.Get(Side.Scp).Any())
                        {
                            Player scpPlayer = Player.Get(Side.Scp).Where(p => p.Role.Type != RoleTypeId.Scp079).ToList().RandomItem();
                            ev.Player.Position = scpPlayer.Position;
                            SendBroadcast(ev.Player, _tr.TPToRandomSCPMessage);
                        }
                        else
                        {
                            ev.Player.Health -= 15;
                            if (ev.Player.Health < 0) ev.Player.Kill(DamageType.Unknown);
                            SendBroadcast(ev.Player, _tr.SmallDamageMessage);
                        }
                        break;
                    case 10:
                        ev.Player.Hurt(ev.Player.Health - 1);
                        SendBroadcast(ev.Player, _tr.HugeDamageMessage);
                        break;
                    case 11:
                        Scp244 vase = (Scp244)Item.Create(ItemType.SCP244a);
                        vase.Primed = true;
                        vase.CreatePickup(ev.Player.Position);
                        SendBroadcast(ev.Player, _tr.PrimedVaseMessage);
                        break;
                    case 12:
                        ev.Player.PlaceTantrum();
                        SendBroadcast(ev.Player, _tr.ShitPantsMessage);
                        break;
                    case 13:
                        var name = _scpNames.ToList().RandomItem();
                        Cassie.MessageTranslated($"scp {name.Key} successfully terminated by automatic security system",$"{name.Value} successfully terminated by Automatic Security System.");
                        SendBroadcast(ev.Player, _tr.FakeSCPKillMessage);
                        break;
                }
            }

            if (_cfg.RemoveCoinOnThrow)
            {
                ev.Player.RemoveHeldItem();
            }
        }

        public void OnSpawningItem(SpawningItemEventArgs ev)
        {
            if (!_cfg.SpawnDefaultCoins && ev.Pickup.Type == ItemType.Coin)
            {
                ev.IsAllowed = false;
            }

            if (_cfg.ItemToReplace != ItemType.None && ev.Pickup.Type == _cfg.ItemToReplace)
            {
                ev.IsAllowed = false;
                Pickup.CreateAndSpawn(ItemType.Coin, ev.Pickup.Position, new Quaternion());
            }
        }

        public void OnInteractingDoorEventArgs(InteractingDoorEventArgs ev)
        {
            foreach (Pickup pickup in ev.Door.Room.Pickups)
            {
                if (pickup.IsLocked && pickup.Type == _cfg.ItemToReplace)
                {
                    pickup.Destroy();
                    Pickup.CreateAndSpawn(ItemType.Coin, pickup.RelativePosition.Position, new Quaternion());
                }
            }
        }
    }
}