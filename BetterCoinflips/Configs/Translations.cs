using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace BetterCoinflips.Configs
{
    public class Translations : ITranslation
    {
        [Description("This is added to the effect message if the coin breaks.")]
        public string CoinBreaksMessage { get; set; } = "\nTwoja moneta się zużyła i się rozpadła.";
        
        [Description("The broadcast message when a coin is registered with no uses.")]
        public string CoinNoUsesMessage { get; set; } = "Twoja moneta nie miała żadnych użyć!";

        [Description("Message sent to the user who received a random coin.")]
        public string RandomCoinMessage { get; set; } = "Otrzymałeś losową monetę!";

        [Description("Message sent to the player when a random coin appears at their feet if there is no space in the inventory.")]
        public string RandomCoinDropMessage { get; set; } = "Nie masz miejsca w ekwipunku, więc losowa moneta pojawiła się pod twoimi nogami!";
        public List<string> HintMessages { get; set; } = new()
        {
            "Twoja moneta wylądowała na reszce.",
            "Twoja moneta wylądowała na orzełku."
        };
        
        [Description("Here you can set the message for each of these good coin effects.")]
        public string TossOnCooldownMessage { get; set; } = "Nie możesz jeszcze rzucić monetą.";
        public string RandomCardMessage { get; set; } = "Otrzymałeś losową kartę dostępu!";
        public string MediKitMessage { get; set; } = "Otrzymałeś zestaw medyczny!";
        public string TpToEscapeMessage { get; set; } = "Teraz możesz uciec! Tego przecież chciałeś prawda?";
        public string MagicHealMessage { get; set; } = "Zostałeś magicznie uleczony!";
        public string HealthIncreaseMessage { get; set; } = "Otrzymałeś 10% więcej HP!";
        public string RandomsScpItem { get; set; } = "Dostałeś losowy przedmiot SCP!";
        public string RandomGoodEffectMessage { get; set; } = "Otrzymałeś losowy efekt.";
        public string WallHackMessage { get; set; } = "Otrzymałeś możliwość widzenia przez ściany na 15 sekund!";
        public string PinkCandyMessage { get; set; } = "Dostałeś ładnego cukierka!";
        public string BadRevoMessage { get; set; } = "Czy to rewolwer?";
        public string SpawnHidMessage { get; set; } = "Czy właśnie dostałeś MICRO HIDA!?";
        public string ForceRespawnMessage { get; set; } = "Ktoś się zrespił... prawdopodobnie.";
        public string SizeChangeMessage { get; set; } = "Zostałeś zmniejszony.";
        public string RandomItemMessage { get; set; } = "Otrzymałeś losowy przedmiot!";
        public string AmmoRefillMessage { get; set; } = "Twoja amunicja została odnowiowa!";
        public string TemporaryGodmodeMessage { get; set; } = "Jesteś nieśmiertelny przez 5 sekund!";
        public string KeycardUpgradedMessage { get; set; } = "Twoja karta została ulepszona!";
        public string NoKeycardMessage { get; set; } = "Nie masz przy sobie żadnej karty do ulepszenia!";
        public string AllPositiveEffectsMessage { get; set; } = "Otrzymałeś wszystkie pozytywne efekty na 60 sekund!";
        public string RandomHpMessage { get; set; } = "Otrzymałeś losową liczbę HP";
        public string ThousandHpMessage { get; set; } = "Otrzymałeś 1000 HP na {duration} sekund!";
        public string RandomGeneratorActivationMessage { get; set; } = "Losowy generator został aktywowany.";
        public string TeleportToSpawnPlayerMessage { get; set; } = "Zostałeś przeteleportowany na swoje początkowe miejsce spawnu!";
        public string DominoEffectMessage { get; set; } = "Wszyscy gracze w promieniu 10 metrów otrzymali losowy efekt!";
        public string TimeLoopMessage { get; set; } = "Za 10 sekund wrócisz do tej pozycji!";
        public string TimeLoopSinglePlayerTeleportedMessage { get; set; } = "Zostałeś teleportowany do swojej pozycji sprzed 10 sek!";

        [Description("Here you can set the message for each of these bad coin effects.")]
        public string HpReductionMessage { get; set; } = "Twoje HP zostało zmniejszone o 30%.";
        public string TpToClassDCellsMessage { get; set; } = "Zostałeś przeteleportowany do cel klas D.";
        public string TpToClassDCellsAfterWarheadMessage { get; set; } = "Zostałeś przeteleportowany do strefy radioaktywnej.";
        public string RandomBadEffectMessage { get; set; } = "Otrzymałeś losowy efekt.";
        public string WarheadStopMessage { get; set; } = "Głowica została zatrzymana.";
        public string WarheadStartMessage { get; set; } = "Głowica została uruchomiona.";        
        public string RandomTeleportMessage { get; set; } = "Zostałeś losowo przeteleportowany.";
        public string InventorySwapOnePlayerMessage { get; set; } = "Nie możesz się z nikim zamienić, więc tracisz zdrowie.";       
        public string HandcuffMessage { get; set; } = "Zostałeś aresztowany za popełnienie zbrodni... czy coś. (15s)";        
        public string RandomTeleportWarheadDetonatedMessage { get; set; } = "Głowica została zdetonowana, więc dostałeś tylko cukierka.";
        public string LightsOutMessage { get; set; } = "Boisz się ciemności?";
        public string LiveGrenadeMessage { get; set; } = "Uważaj na głowe!";
        public string TrollFlashMessage { get; set; } = "słyszałeś coś?";
        public string TpToRandomScpMessage { get; set; } = "Zostałeś przeteleportowany do losowego SCP.";
        public string SmallDamageMessage { get; set; } = "Straciłeś 15 HP.";
        public string HugeDamageMessage { get; set; } = "Straciłeś dużo HP";
        public string PrimedVaseMessage { get; set; } = "Nie za zimno?";
        public string ShitPantsMessage { get; set; } = "Właśnie się zesrałeś...";
        public string FakeScpKillMessage { get; set; } = "Chyba właśnie zabiłeś SCP.";
        public string TurnIntoScpMessage { get; set; } = "Zostałeś zamieniony w SCP!";
        public string InventoryResetMessage { get; set; } = "Straciłeś swoje rzeczy.";
        public string ClassSwapMessage { get; set; } = "To się nazywa karta UNO reverse!";
        public string InstantExplosionMessage { get; set; } = "bum.";
        public string PlayerSwapMessage { get; set; } = "Twoja pozycja została zamieniona z innym graczem.";
        public string PlayerSwapIfOneAliveMessage { get; set; } = "Miałeś się zamienić z kimś miejscami, ale nikt inny nie żyje!";
        public string KickMessage { get; set; } = "Do widzenia!";
        public string SpectSwapPlayerMessage { get; set; } = "Zamieniłeś się rolą z obserwatorem!";
        public string SpectSwapSpectMessage { get; set; } = "Zostałeś wybrany jako losowy obserwator, aby zastąpić tego gracza!";
        public string SpectSwapNoSpectsMessage { get; set; } = "Miałeś szczęście, bo nie ma obserwatorów, którzy mogliby cię zastąpić";
        public string TeslaTpMessage { get; set; } = "Jesteś może elektrykiem?";
        public string TeslaTpAfterWarheadMessage { get; set; } = "Zostałeś przeteleportowany do strefy radioaktywnej.";
        public string FakeNtfMessage { get; set; } = "Podobno NTF wchodzi do placówki.";
        public string RandomTeleportationMessage { get; set; } = "Będziesz losowo teleportowany przez następne 20 sekund!";
        public string UpsideDownScaleMessage { get; set; } = "Twoja postać obróciła się góry nogami!";
        public string WalkingTimeBombMessage { get; set; } = "Jesteś chodzącą bombą";

        [Description("This message will be broadcast to both players.")]
        public string InventorySwapMessage { get; set; } = "Twój ekwipunek został zmieniony z losowym graczem.";
        public string TeleportToSpawnMessage { get; set; } = "Wszyscy gracze zostali przeteleportowani na swoje początkowe miejsca spawnu!";
        public string LightZoneDecontaminationMessage { get; set; } = "Dekontaminacja strefy Light na 5 sekund.";
        public string ZoneDoorLockMessage { get; set; } = "Wszystkie drzwi w tej strefie zostały zablokowane na 10 sekund!";
        public string RandomItemDropMessage { get; set; } = "Twoje przedmioty zaczynają wypadać z ekwipunku!";
        public string DominoEffectReceivedMessage { get; set; } = "Otrzymałeś losowy efekt od pobliskiego gracza!";
        public string TimeLoopTeleportingMessage { get; set; } = "Za 10 sekund wszyscy gracze zostaną przeniesieni do miejsca, w którym byli 10 sekund temu.";
        public string TimeLoopAllPlayersMessage { get; set; } = "Wszyscy gracze zostali teleportowani do swoich poprzednich pozycji!";

    }
}