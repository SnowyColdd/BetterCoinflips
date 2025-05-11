using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace BetterCoinflips.Configs
{
    public class Translations : ITranslation
    {
        [Description("This is added to the effect message if the coin breaks.")]
        public string CoinBreaksMessage { get; set; } = "\nYour coin wore out and broke.";
        
        [Description("The broadcast message when a coin is registered with no uses.")]
        public string CoinNoUsesMessage { get; set; } = "Your coin had no uses!";

        [Description("Message sent to the user who received a random coin.")]
        public string RandomCoinMessage { get; set; } = "You received a random coin!";

        [Description("Message sent to the player when a random coin appears at their feet if there is no space in the inventory.")]
        public string RandomCoinDropMessage { get; set; } = "No space in your inventory, so the random coin dropped at your feet!";
        public List<string> HintMessages { get; set; } = new()
        {
            "Your coin landed on tails.",
            "Your coin landed on heads."
        };
        
        [Description("Here you can set the message for each of these good coin effects.")]
        public string TossOnCooldownMessage { get; set; } = "You can't throw the coin yet.";
        public string RandomCardMessage { get; set; } = "You received a random access card!";
        public string MediKitMessage { get; set; } = "You received a medkit!";
        public string TpToEscapeMessage { get; set; } = "Now you can escape! That's what you wanted, right?";
        public string MagicHealMessage { get; set; } = "You were magically healed!";
        public string HealthIncreaseMessage { get; set; } = "You gained 10% more HP!";
        public string RandomsScpItem { get; set; } = "You got a random SCP item!";
        public string RandomGoodEffectMessage { get; set; } = "You received a random effect.";
        public string WallHackMessage { get; set; } = "You can see through walls for 15 seconds!";
        public string PinkCandyMessage { get; set; } = "You got a pretty candy!";
        public string BadRevoMessage { get; set; } = "Is that a revolver?";
        public string RandomSpecialWeaponMessage { get; set; } = "Did you just get SPECIAL WEAPON!?";
        public string ForceRespawnMessage { get; set; } = "Someone respawned... probably.";
        public string SizeChangeMessage { get; set; } = "You've been shrunk.";
        public string RandomItemMessage { get; set; } = "You got a random item!";
        public string AmmoRefillMessage { get; set; } = "Your ammo has been refilled!";
        public string TemporaryGodmodeMessage { get; set; } = "'You're invincible for 5 seconds!";
        public string KeycardUpgradedMessage { get; set; } = "Your card has been upgraded!";
        public string NoKeycardMessage { get; set; } = "You don't have a card to upgrade!";
        public string AllPositiveEffectsMessage { get; set; } = "You received all positive effects for 60 seconds!";
        public string RandomHpMessage { get; set; } = "You received a random amount of HP.";
        public string ThousandHpMessage { get; set; } = "You got 1000 HP for {duration} seconds!";
        public string RandomGeneratorActivationMessage { get; set; } = "A random generator has been activated.";
        public string TeleportToSpawnPlayerMessage { get; set; } = "You've been teleported to your original spawn!";
        public string DominoEffectMessage { get; set; } = "All players within 10 meters received a random effect!";
        public string TimeLoopMessage { get; set; } = "In 10 seconds you'll return to this position!";
        public string TimeLoopSinglePlayerTeleportedMessage { get; set; } = "You were teleported to your position from 10 seconds ago!";

        [Description("Here you can set the message for each of these bad coin effects.")]
        public string HpReductionMessage { get; set; } = "Your HP has been reduced by 30%.";
        public string TpToClassDCellsMessage { get; set; } = "You were teleported to the Class D cells.";
        public string TpToClassDCellsAfterWarheadMessage { get; set; } = "You were teleported to a radioactive area.";
        public string RandomBadEffectMessage { get; set; } = "You received a random effect.";
        public string WarheadStopMessage { get; set; } = "The warhead has been stopped.";
        public string WarheadStartMessage { get; set; } = "The warhead has been launched.";        
        public string RandomTeleportMessage { get; set; } = "You were teleported randomly.";
        public string InventorySwapOnePlayerMessage { get; set; } = "No one to swap with, so you lose health.";       
        public string HandcuffMessage { get; set; } = "You've been arrested for... something. (15s)";        
        public string RandomTeleportWarheadDetonatedMessage { get; set; } = "Warhead exploded, so you only got a candy.";
        public string LightsOutMessage { get; set; } = "Are you afraid of the dark?";
        public string LiveGrenadeMessage { get; set; } = "Watch your head!";
        public string TrollFlashMessage { get; set; } = "Did you hear something?";
        public string TpToRandomScpMessage { get; set; } = "You were teleported to a random SCP.";
        public string SmallDamageMessage { get; set; } = "You lost 15 HP.";
        public string HugeDamageMessage { get; set; } = "You lost a lot of HP.";
        public string PrimedVaseMessage { get; set; } = "Not too cold, huh?";
        public string ShitPantsMessage { get; set; } = "You just pooped your pants...";
        public string FakeScpKillMessage { get; set; } = "You probably killed an SCP. Or not.";
        public string TurnIntoScpMessage { get; set; } = "You turned into an SCP!";
        public string InventoryResetMessage { get; set; } = "You lost your stuff.";
        public string ClassSwapMessage { get; set; } = "UNO reverse card played!";
        public string InstantExplosionMessage { get; set; } = "Boom.";
        public string PlayerSwapMessage { get; set; } = "You swapped positions with another player.";
        public string PlayerSwapIfOneAliveMessage { get; set; } = "You tried to swap, but no one else is alive!";
        public string KickMessage { get; set; } = "Goodbye!";
        public string SpectSwapPlayerMessage { get; set; } = "You swapped roles with a spectator!";
        public string SpectSwapSpectMessage { get; set; } = "You were picked as a random spectator to replace this player!";
        public string SpectSwapNoSpectsMessage { get; set; } = "Lucky you – no spectators to replace you.";
        public string TeslaTpMessage { get; set; } = "Are you an electrician?";
        public string TeslaTpAfterWarheadMessage { get; set; } = "You were teleported to a radioactive zone.";
        public string FakeNtfMessage { get; set; } = "Supposedly, the NTF is entering the facility.";
        public string RandomTeleportationMessage { get; set; } = "You'll be randomly teleported for the next 20 seconds!";
        public string UpsideDownScaleMessage { get; set; } = "Your character flipped upside down!";
        public string WalkingTimeBombMessage { get; set; } = "You're a walking time bomb.";

        [Description("This message will be broadcast to both players.")]
        public string InventorySwapMessage { get; set; } = "Your inventory was swapped with a random player.";
        public string TeleportToSpawnMessage { get; set; } = "All players were teleported to their original spawn!";
        public string LightZoneDecontaminationMessage { get; set; } = "Light zone will decontaminate in 5 seconds.";
        public string ZoneDoorLockMessage { get; set; } = "All doors in this zone are locked for 10 seconds!";
        public string RandomItemDropMessage { get; set; } = "Your items are falling out of your inventory!";
        public string DominoEffectReceivedMessage { get; set; } = "You got a random effect from a nearby player!";
        public string TimeLoopTeleportingMessage { get; set; } = "In 10 seconds, everyone will return to their position from 10 seconds ago.";
        public string TimeLoopAllPlayersMessage { get; set; } = "All players were teleported to their previous positions!";

    }
}