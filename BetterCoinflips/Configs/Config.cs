using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;

namespace BetterCoinflips.Configs
{
    public class Config : IConfig
    {
        [Description("Whether or not the plugin should be enabled. Default: true")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether or not debug logs should be shown. Default: false")]
        public bool Debug { get; set; } = false;

        [Description("The amount of base game spawned coins that should be removed. Default: 4")]
        public int DefaultCoinsAmount { get; set; } = 4;

        [Description("The ItemType of the item to be replaced with a coin and the amount to be replaced, the item is supposed to be something found in SCP pedestals.")]
        public Dictionary<ItemType, int> ItemToReplace { get; set; } = new()
        {
            { ItemType.SCP500, 1 }
        };

        [Description("The boundaries of the random range of throws each coin will have before it breaks. The upper bound is exclusive.")]
        public List<int> MinMaxDefaultCoins { get; set; } = new()
        {
            2, 
            4
        };

        [Description("Time in seconds between coin toses. Default: 5")]
        public double CoinCooldown { get; set; } = 5;

        [Description("The duration in seconds of the broadcast informing you about your 'reward'. Default: 5")]
        public ushort BroadcastTime { get; set; } = 5;
        
        [Description("The duration in seconds of the hint telling you if you got heads or tails. Set to 0 or less to disable.")]
        public float HintDuration { get; set; } = 5;

        [Description("The duration in seconds of the map blackout. Default: 10")]
        public float MapBlackoutTime { get; set; } = 10;

        [Description("The fuse time of the grenade falling on your head. Default: 3.25")]
        public double LiveGrenadeFuseTime { get; set; } = 3.25;

        [Description("Determines the behavior of size reduction: 0 - Until first death, 1 - Persistent until end of game, 2 - Growing with each respawn. Default: 0")]
        public int SizeReductionBehavior { get; set; } = 0;

        [Description("The frequency of growth when it is small. Default: 0.2")]
        public float GrowthFrequency { get; set; } = 0.2f;

        [Description("Time in minutes how often a random player will receive a coin. Set 0 to disable. Default: 0")]
        public int RandomCoinInterval { get; set; } = 0;

        [Description("Duration in seconds for which the player will have 1000 HP. Default: 15")]
        public int ThousandHpDuration { get; set; } = 15;

        [Description("Determines whether only the player who flipped the coin should be teleported or all players except spectators and SCP-079. Default: false")]
        public bool TeleportAllPlayersOnCoinFlip { get; set; } = false; // false = teleport only the player, true = teleport all players

        [Description("List of bad effects that can be applied to the players. List available at: https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.EffectType.html")]
        public HashSet<EffectType> BadEffects { get; set; } = new()
        {
            EffectType.Asphyxiated,
            EffectType.Bleeding,
            EffectType.Blinded,
            EffectType.Burned,
            EffectType.Concussed,
            EffectType.Corroding,
            EffectType.CardiacArrest,
            EffectType.Deafened,
            EffectType.Disabled,
            EffectType.Ensnared,
            EffectType.Exhausted,
            EffectType.Flashed,
            EffectType.Hemorrhage,
            EffectType.Hypothermia,
            EffectType.InsufficientLighting,
            EffectType.Poisoned,
            EffectType.SeveredHands,
            EffectType.SinkHole,
            EffectType.Stained,
            EffectType.Traumatized
        };
        
        [Description("List of good effects that can be applied to the players. List available at: https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.EffectType.html")]
        public HashSet<EffectType> GoodEffects { get; set; } = new()
        {
            EffectType.BodyshotReduction,
            EffectType.DamageReduction,
            EffectType.Invigorated,
            EffectType.Invisible,
            EffectType.MovementBoost,
            EffectType.RainbowTaste,
            EffectType.Scp207,
            EffectType.Vitality,
            EffectType.Ghostly,
            EffectType.SilentWalk,
        };

        [Description("The % chance of receiving a Facility Manager keycard instead of a Containment Engineer one.")]
        public int RedCardChance { get; set; } = 15;

        [Description("The kick reason.")] 
        public string KickReason { get; set; } = "Moneta postanowiła wywalić cię z serwera.";

        [Description("The list of SCP's that you can turn into by using the coin.")]
        public HashSet<RoleTypeId> ValidScps { get; set; } = new()
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp096,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
            RoleTypeId.Scp0492,
            RoleTypeId.Scp939,
            RoleTypeId.Scp079,
        };

        [Description("List of ignored roles for the PlayerSwap effect (#17)")]
        public HashSet<RoleTypeId> PlayerSwapIgnoredRoles { get; set; } = new()
        {
            RoleTypeId.Spectator,
            RoleTypeId.Filmmaker,
            RoleTypeId.Overwatch,
            RoleTypeId.Scp079,
            RoleTypeId.Tutorial,
        };

        [Description("List of ignored roles for the InventorySwap effect (#17)")]
        public HashSet<RoleTypeId> InventorySwapIgnoredRoles { get; set; } = new()
        {
            RoleTypeId.Spectator,
            RoleTypeId.Filmmaker,
            RoleTypeId.Overwatch,
            RoleTypeId.Scp079,
            RoleTypeId.Tutorial,
            RoleTypeId.Scp049,
            RoleTypeId.Scp079,
            RoleTypeId.Scp096,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
            RoleTypeId.Scp0492,
            RoleTypeId.Scp939,
            RoleTypeId.Scp3114,
        };
        
        public HashSet<ItemType> ItemsToGive { get; set; } = new()
        {
            ItemType.Adrenaline,
            ItemType.Coin,
            ItemType.Flashlight,
            ItemType.Jailbird,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.Radio,
            ItemType.ArmorCombat,
            ItemType.ArmorHeavy,
            ItemType.ArmorLight,
            ItemType.GrenadeFlash,
            ItemType.GrenadeHE,
            ItemType.GunA7,
            ItemType.GunCom45,
            ItemType.GunCrossvec,
            ItemType.GunLogicer,
            ItemType.GunRevolver,
            ItemType.GunShotgun,
            ItemType.GunAK,
            ItemType.GunCOM15,
            ItemType.GunCOM18,
            ItemType.GunE11SR,
            ItemType.GunFSP9,
            ItemType.GunFRMG0,
        };

        public HashSet<RoomType> RoomsToTeleport { get; set; } = new()
        {
            RoomType.EzCafeteria,
            RoomType.EzCheckpointHallwayA,
            RoomType.EzCheckpointHallwayB,
            RoomType.EzConference,
            RoomType.EzCrossing,
            RoomType.EzCurve,
            RoomType.EzDownstairsPcs,
            RoomType.EzGateA,
            RoomType.EzGateB,
            RoomType.EzIntercom,
            RoomType.EzPcs,
            RoomType.EzStraight,
            RoomType.EzTCross,
            RoomType.EzUpstairsPcs,
            RoomType.EzVent,
            RoomType.Hcz049,
            RoomType.Hcz079,
            RoomType.Hcz096,
            RoomType.Hcz106,
            RoomType.Hcz939,
            RoomType.HczArmory,
            RoomType.HczCrossing,
            RoomType.HczCurve,
            RoomType.HczElevatorA,
            RoomType.HczElevatorB,
            RoomType.HczEzCheckpointA,
            RoomType.HczEzCheckpointB,
            RoomType.HczHid,
            RoomType.HczNuke,
            RoomType.HczStraight,
            RoomType.HczTesla,
            RoomType.HczTestRoom,
            RoomType.Lcz173,
            RoomType.Lcz330,
            RoomType.Lcz914,
            RoomType.LczAirlock,
            RoomType.LczArmory,
            RoomType.LczCafe,
            RoomType.LczCheckpointA,
            RoomType.LczCheckpointB,
            RoomType.LczClassDSpawn,
            RoomType.LczCrossing,
            RoomType.LczCurve,
            RoomType.LczGlassBox,
            RoomType.LczPlants,
            RoomType.LczStraight,
            RoomType.LczTCross,
            RoomType.LczToilets,
            RoomType.Surface,
        };

        [Description("The chance of these good effects happening. It's a proportional chance not a % chance.")]
        public int RandomCardChance { get; set; } = 15;
        public int MedicalKitChance { get; set; } = 30;
        public int TpToEscapeChance { get; set; } = 5;
        public int HealChance { get; set; } = 20;
        public int MoreHpChance { get; set; } = 20;
        public int RandomsScpItemChance { get; set; } = 15;
        public int RandomGoodEffectChance { get; set; } = 30;
        public int WallHackChance { get; set; } = 5;
        public int PinkCandyChance { get; set; } = 20;
        public int BadRevoChance { get; set; } = 5;
        public int SpawnHidChance { get; set; } = 1;
        public int ForceRespawnChance { get; set; } = 15;
        public int SizeChangeChance { get; set; } = 20;
        public int RandomItemChance { get; set; } = 35;
        public int AmmoRefillChance{ get; set; } = 25;
        public int TemporaryGodmodeChance { get; set; } = 5;
        public int KeycardUpgradeChance { get;set; } = 20;
        public int AllPositiveEffectsChance { get; set; } = 5;
        public int RandomHpChance { get; set; } = 15;
        public int ThousandHpChance { get; set; } = 15;
        public int RandomGeneratorActivationChance { get; set; } = 20;
        public int DominoEffectChance { get; set; } = 15;
        public int TimeLoopChance { get; set; } = 10;

        [Description("The chance of these bad effects happening. It's a proportional chance not a % chance.")]
        public int HpReductionChance { get; set; } = 20;
        public int TpToClassDCellsChance { get; set; } = 5;
        public int RandomBadEffectChance { get; set; } = 25;
        public int WarheadChance { get; set; } = 10;
        public int LightsOutChance { get; set; } = 15;
        public int LiveHeChance { get; set; } = 30;
        public int TrollFlashChance { get; set; } = 35;
        public int ScpTpChance { get; set; } = 10;
        public int OneHpLeftChance { get; set; } = 10;
        public int PrimedVaseChance { get; set; } = 20;
        public int ShitPantsChance { get; set; } = 40;
        public int FakeCassieChance { get; set; } = 20;
        public int TurnIntoScpChance { get; set; } = 5;
        public int InventoryResetChance { get; set; } = 10;
        public int ClassSwapChance { get; set; } = 20;
        public int InstantExplosionChance { get; set; } = 15;
        public int PlayerSwapChance { get; set; } = 25;
        public int KickChance { get; set; } = 5;
        public int SpectSwapChance { get; set; } = 10;
        public int TeslaTpChance { get; set; } = 10;
        public int InventorySwapChance { get; set; } = 20;
        public int HandcuffChance { get; set; } = 15;
        public int RandomTeleportChance { get; set; } = 15;
        public int TeleportToSpawnChance { get; set; } = 5;
        public int FakeNtfChance { get; set;} = 5;
        public int LightZoneDecontaminationChance {  get; set; } = 10;
        public int RandomTeleportationChance { get; set; } = 15;
        public int UpsideDownScaleChance { get; set; } = 20;
        public int ZoneDoorLockChance { get; set; } = 15;
        public int RandomItemDropChance { get; set; } = 15;
        public int WalkingTimeBombChance { get; set; } = 20;
    }
}
