using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace BetterCoinflips.Configs
{
    public class Translations : ITranslation
    {
        [Description("This is added to the effect message if the coin breaks.")]
        public string CoinBreaksMessage { get; set; } = "\nTwoja moneta zużyła się zbyt mocno i się rozpadła.";
        
        [Description("The broadcast message when a coin is registered with no uses.")]
        public string CoinNoUsesMessage { get; set; } = "Twoja moneta nie miała żadnych użyć!";

        [Description("Message sent to the user who received a random coin.")]
        public string RandomCoinMessage { get; set; } = "Otrzymałeś losową monetę!";

        [Description("Message sent to the player when a random coin appears at their feet if there is no space in the inventory.")]
        public string RandomCoinDropMessage { get; set; } = "Nie masz miejsca w ekwipunku, więc losowa moneta pojawiła się pod twoimi nogami!";
        public List<string> HintMessages { get; set; } = new()
        {
            "Twoja moneta wylądowała na reszce.",
            "Twoja moneta wylądowałą na orzełku."
        };
        
        [Description("Here you can set the message for each of these good coin effects.")]
        public string TossOnCooldownMessage { get; set; } = "Nie możesz jeszcze rzucić monetą.";
        public string RedCardMessage { get; set; } = "Otrzymujesz kartę admininstatora placówki!";
        public string ContainmentEngineerCardMessage { get; set; } = "Otrzymujesz kartę technika zapezbieczeń!";
        public string MediKitMessage { get; set; } = "Otrzymałeś zestaw medyczny!";
        public string TpToEscapeMessage { get; set; } = "Teraz możesz uciec! Tego przecież chciałeś prawda?";
        public string MagicHealMessage { get; set; } = "Zostałeś magicznie uleczony!";
        public string HealthIncreaseMessage { get; set; } = "Otrzymałeś 10% więcej HP!";
        public string NeatHatMessage { get; set; } = "Dostałeś kapelusz!";
        public string RandomGoodEffectMessage { get; set; } = "Otrzymałeś losowy efekt.";
        public string OneAmmoLogicerMessage { get; set; } = "Dostałeś broń.";
        public string LightbulbMessage { get; set; } = "Dostałeś błyszczącą żarówkę!";
        public string PinkCandyMessage { get; set; } = "Dostałeś ładnego cukierka!";
        public string BadRevoMessage { get; set; } = "Co to za obominacja!?";
        public string EmptyHidMessage { get; set; } = "Czy właśnie dostałeś MICRO HIDA!?";
        public string ForceRespawnMessage { get; set; } = "Ktoś się zrespił... prawdopodobnie.";
        public string SizeChangeMessage { get; set; } = "Zostałeś zmniejszony.";
        public string RandomItemMessage { get; set; } = "Dostałeś losowy przedmiot!";

        
        [Description("Here you can set the message for each of these bad coin effects.")]
        public string HpReductionMessage { get; set; } = "Twoje HP zostało zmniejszone o 30%.";
        public string TpToClassDCellsMessage { get; set; } = "Zostałeś przeteleportowany do cel klas D.";
        public string TpToClassDCellsAfterWarheadMessage { get; set; } = "Zostałeś przeteleportowany do strefy radioaktywnej.";
        public string RandomBadEffectMessage { get; set; } = "Dostałeś losowy efekt.";
        public string WarheadStopMessage { get; set; } = "Głowica została zatrzymana.";
        public string WarheadStartMessage { get; set; } = "Głowica została uruchomiona.";
        public string LightsOutMessage { get; set; } = "Niech się stanie ciemność!";
        public string LiveGrenadeMessage { get; set; } = "Uważaj na głowe!";
        public string TrollFlashMessage { get; set; } = "słyszałeś coś?";
        public string TpToRandomScpMessage { get; set; } = "Zostałeś przeteleportowany do SCP.";
        public string SmallDamageMessage { get; set; } = "Straciłeś 15 HP.";
        public string HugeDamageMessage { get; set; } = "Straciłeś dużo HP";
        public string PrimedVaseMessage { get; set; } = "Nie za zimno?";
        public string ShitPantsMessage { get; set; } = "Właśnie się zesrałeś...";
        public string FakeScpKillMessage { get; set; } = "Czy właśnie zabiłeś SCP?!";
        public string TurnIntoScpMessage { get; set; } = "Zostałeś zamieniony w SCP!";
        public string InventoryResetMessage { get; set; } = "Straciłeś swoje rzeczy.";
        public string ClassSwapMessage { get; set; } = "To się nazywa karta UNO reverse!";
        public string InstantExplosionMessage { get; set; } = "bum.";
        public string PlayerSwapMessage { get; set; } = "Twoja pozycja została zamieniona z innym graczem.";
        public string PlayerSwapIfOneAliveMessage { get; set; } = "Miałeś się zamienić z kimś miejscami, ale nikt inny nie żyje!";
        public string KickMessage { get; set; } = "Do widzenia!";
        public string SpectSwapPlayerMessage { get; set; } = "Właśnie poprawiłeś komuś runde!";
        public string SpectSwapSpectMessage { get; set; } = "Zostałeś wybrany jako losowy obserwator, aby zastąpić tego gracza!";
        public string SpectSwapNoSpectsMessage { get; set; } = "Miałeś szczęście, bo nie ma obserwatorów, którzy mogliby cię zastąpić";
        public string TeslaTpMessage { get; set; } = "Lubisz prąd?";
        public string TeslaTpAfterWarheadMessage { get; set; } = "Zostałeś przeteleportowany do strefy radioaktywnej.";
        
        [Description("This message will be broadcast to both players.")]
        public string InventorySwapMessage { get; set; } = "Twój ekwipunek został zmieniony z losowym graczem.";
        public string InventorySwapOnePlayerMessage { get; set; } = "Nie możesz się z nikim zamienić, więc tracisz zdrowie.";
        public string RandomTeleportMessage { get; set; } = "Zostałeś losowo przeteleportowany.";
        public string RandomTeleportWarheadDetonatedMessage { get; set; } = "Głowica została zdetonowana, więc dostałeś tylko cukierka.";
        public string HandcuffMessage { get; set; } = "Zostałeś aresztowany za popełnienie zbrodni... czy coś.";
        public string TeleportToSpawnMessage { get; set; } = "Wszyscy zostali przeteleportowani na swoje początkowe miejsce respawnu!";
        public string FakeNtfMessage { get; set; } = "Słyszałeś coś o NTF?";
    }
}