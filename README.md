<a href="https://github.com/snowycoldd/BetterCoinflips/releases"><img src="https://img.shields.io/github/downloads/snowycoldd/BetterCoinflips/total?label=Downloads" alt="Downloads"></a>  

# Fork
Ten plugin jest forkiem [BetterCoinflips](https://github.com/Mikihero/BetterCoinflips) autorstwa [Mikihero](https://github.com/Mikihero) który jest przepisany na Polski oraz posiada usprawnienia.

## BetterCoinflips
  
Jest to plugin do gry SCP:SL który dodaje nagrode lub antynagrode za rzut monetą w grze. Za każdym razem gdy rzucisz monetą dostaniesz losowy efekt, zależny od wyniku rzutu monetą.

- Plugin testowany na wersji gry **14.0.0**

## Cechy pluginu:

- Za każdym razem kiedy gracz rzuci monetą jedna z poniższych rzeczy się wydarzy:  
 1. Otrzyma losową kartę dostępu.  
 2. Otrzyma 'zestaw medyczny' składający sie z apteczki oraz tabletek na ból.
 3. Otrzyma teleport do drzwi strefowych.
 4. Otrzyma pełne leczenie.
 5. Jego HP zostanie zwiększone o 10%.
 6. Otrzyma losowy item SCP.
 7. Otrzyma losowy dobry efekt na 5 sekund.
 8. Dostanie możliwość widzenia graczy przez ściany przez 15 sekund.   
 9. Otrzyma różowego cukierka. 
 10. Otrzyma rewolwer z najgorszymi dodatkami jakie można mieć. 
 11. Otrzyma naładowanego micro hida.
 12. Natychmiastowy respawn MTF/CI.
 13. Otrzyma zmniejszenie postaci do skali 1.3/0.5/1.3.
 14. Otrzyma losowy item.
 15. Otrzyma tymczasową nieśmiertelność na 5 sekund.
 16. Jego karta dostępu zostanie ulepszona do wyższego poziomu.
 17. Cała jego amunicja zostanie uzupełniona.
 18. Otrzyma losową liczbę HP.
 19. Otrzyma 1000 HP na określony czas.
 20. Aktywuje losowy generator.
 21. Efekt domina - gracze w pobliżu otrzymują losowe pozytywne efekty.
 22. Pętla czasowa - teleportuje graczy z powrotem na ich pozycje po 10 sekundach.

- Za każdym razem, gdy gracz rzuci monetą i wyląduje na reszce, wydarzy się jedna z poniższych sytuacji:  
 1. Jego HP zostanie zmniejszone o 30%.  
 2. Zostanie przeteleportowany do cel klas D.  
 3. Dostanie losowy zły efekt na 5 sekund.  
 4. Głowica Alpha Warhead zostanie włączona lub wyłaczona w zależności od jej aktualnego stanu.  
 5. Światła na całej mapie zostaną wyłączone na 10 sekund.  
 6. Nad głową pojawi się niezabezpieczony granat.
 7. Nad głową pojawi się niezabezpieczony granat błyskowy.
 8. Zostanie przeteleportowany do SCP gdy jakiś jest żywy, jeśli nie to straci 15HP.
 9. Traci wszystko i zostaje mu 1HP.
 10. Dostaje aktywonane SCP-244 pod nogami.
 11. Otrzymuje furie SCP-173.
 12. Fałszywa cassie wysyła komunikat że SCP zostaje zabite przez tesle.
 13. Zostaje zmieniony w losowego SCP.
 14. Jego ekwipunek zostaje zresetowany.
 15. Jego rola zostaje zmieniona na przeciwną strone (klasa D - naukowiec, MTF - CI itp.)
 16. Na jego głowie pojawia się granat który natychmiastowo wybucha.
 17. Zostaje zamieniony miejscem z innym graczem.
 18. Zostanie wyrzucony z serwera.
 19. Zostanie zamieniony z losowym obserwujacym.
 20. Zostanie przeteleportowany do losowej tesli.
 21. Jego ekwipunek zostanie zamieniony z ekwipunkiem innego gracza.
 22. Zostanie przeteleportowany do losowego pomieszczenia.
 23. Zostanie skuty i straci swoje itemy.
 24. Fałszywy komunikat o przybyciu MTF.
 25. Wszyscy gracze w strefie lekkiej otrzymają efekt dekontaminacji.
 26. Będzie losowo teleportowany po mapie co 5 sekund przez 20 sekund.
 27. Zostanie obrócony do góry nogami na 30 sekund.
 28. Zablokuje wszystkie drzwi w obecnej strefie na 10 sekund.
 29. Zacznie losowo wypadać przedmioty z ekwipunku.
 30. Stanie się chodzącą bombą z opóźnionym wybuchem.

- Plugin zapobiega pojawieniu się określonej ilości monet na mapie.
- Plugin zastąpi specyficzną liczbę wygranych itemów (Domyślnie SCP-500) na monete.
- Plugin przypisze losową liczbę użyć do każdej rzuconej monety. Kwotę tę można odczytać lub ustawić za pomocą polecenia. Jeśli moneta się skończy, pęknie.

## Komendy

- GetSerial - Otrzymuje numer seryjny przedmiotu trzymanego przez Ciebie lub innego gracza.
- CoinUses - Pobiera lub ustawia liczbę zastosowań danej monety.. Przykład użycia: `coinuses get player 5`, `coin uses set player 4`, `coinuses set serial 10` 

## Permisje

- bc.coinuses.set - daje dostęp do komendy set CoinUses
- bc.coinuses.get - daje dostęp do komendy get CoinUses

## Domyślny config pluginu

```yaml
better_cf:
  # Whether or not the plugin should be enabled. Default: true
  is_enabled: true
  # Whether or not debug logs should be shown. Default: false
  debug: false
  # The amount of base game spawned coins that should be removed. Default: 4
  default_coins_amount: 4
  # The ItemType of the item to be replaced with a coin and the amount to be replaced, the item is supposed to be something found in SCP pedestals.
  item_to_replace:
    SCP500: 1
  # The boundaries of the random range of throws each coin will have before it breaks. The upper bound is exclusive.
  min_max_default_coins:
  - 2
  - 4
  # Time in seconds between coin toses. Default: 5
  coin_cooldown: 5
  # The duration of the broadcast informing you about your 'reward'. Default: 5
  broadcast_time: 5
  # The duration of the hint telling you if you got heads or tails. Set to 0 or less to disable.
  hint_time: 5
  # The duration of the map blackout. Default: 10
  map_blackout_time: 10
  # The fuse time of the grenade falling on your head. Default: 3.25
  live_grenade_fuse_time: 3.25
  # Determines the behavior of size reduction: 0 - Until first death, 1 - Persistent until end of game, 2 - Growing with each respawn. Default: 0
  size_reduction_behavior: 0
  # The frequency of growth when it is small. Default: 0.2
  growth_frequency: 0.2
  # Time in minutes how often a random player will receive a coin. Set 0 to disable. Default: 0
  random_coin_interval: 0
  # Duration in seconds for which the player will have 1000 HP. Default: 15
  thousand_hp_duration: 15
  # Determines whether only the player who flipped the coin should be teleported or all players except spectators and SCP-079. Default: false
  teleport_all_players_on_coin_flip: false
  # List of bad effects that can be applied to the players. List available at: https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.EffectType.html
  bad_effects:
  - Asphyxiated
  - Bleeding
  - Blinded
  - Burned
  - Concussed
  - Corroding
  - CardiacArrest
  - Deafened
  - Disabled
  - Ensnared
  - Exhausted
  - Flashed
  - Hemorrhage
  - Hypothermia
  - InsufficientLighting
  - Poisoned
  - SeveredHands
  - SinkHole
  - Stained
  - Traumatized
  # List of good effects that can be applied to the players. List available at: https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.EffectType.html
  good_effects:
  - BodyshotReduction
  - DamageReduction
  - Invigorated
  - Invisible
  - MovementBoost
  - RainbowTaste
  - Scp207
  - Vitality
  - Ghostly
  - SilentWalk
  # The % chance of receiving a Facility Manager keycard instead of a Containment Engineer one.
  red_card_chance: 15
  # The kick reason.
  kick_reason: 'Moneta postanowiła wywalić cię z serwera.'
  # The chance of these good effects happening. It's a proportional chance not a % chance.
  random_card_chance: 15
  medical_kit_chance: 30
  tp_to_escape_chance: 5
  heal_chance: 20
  more_hp_chance: 20
  random_scp_item_chance: 15
  random_good_effect_chance: 30
  one_ammo_logicer_chance: 5
  pink_candy_chance: 20
  bad_revo_chance: 5
  spawn_hid_chance: 1
  force_respawn_chance: 15
  size_change_chance: 20
  random_item_chance: 35
  ammo_refill_chance: 25
  temporary_godmode_chance: 5 
  keycard_upgrade_chance: 20
  all_positive_effects_chance: 5
  random_hp_chance: 15
  thousand_hp_chance: 15
  random_generator_activation_chance: 20
  domino_effect_chance: 15
  time_loop_chance: 10

  # The chance of these bad effects happening. It's a proportional chance not a % chance.
  hp_reduction_chance: 20
  tp_to_class_d_cells_chance: 5
  random_bad_effect_chance: 25
  warhead_chance: 10
  lights_out_chance: 15
  live_he_chance: 30
  troll_flash_chance: 35
  scp_tp_chance: 10
  one_hp_left_chance: 10
  primed_vase_chance: 20
  shit_pants_chance: 40
  fake_cassie_chance: 20
  turn_into_scp_chance: 5
  inventory_reset_chance: 10
  class_swap_chance: 20
  instant_explosion_chance: 15
  player_swap_chance: 25
  kick_chance: 5
  spect_swap_chance: 10
  tesla_tp_chance: 10
  inventory_swap_chance: 20
  handcuff_chance: 15
  random_teleport_chance: 15
  teleport_to_spawn_chance: 5
  fake_ntf_chance: 5
  light_zone_decontamination_chance: 10
  random_teleportation_chance: 15
  upside_down_scale_chance: 20
  zone_door_lock_chance: 15
  random_item_drop_chance: 15
  walking_time_bomb_chance: 20
```