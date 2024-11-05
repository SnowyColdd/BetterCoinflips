<a href="https://github.com/snowycoldd/BetterCoinflips/releases"><img src="https://img.shields.io/github/downloads/snowycoldd/BetterCoinflips/total?label=Downloads" alt="Downloads"></a>  

# Fork
Ten plugin jest forkiem [BetterCoinflips](https://github.com/Mikihero/BetterCoinflips) autorstwa [Mikihero](https://github.com/Mikihero) który jest przepisany na Polski oraz posiada usprawnienia.

## BetterCoinflips
  
Jest to plugin do gry SCP:SL który dodaje nagrode lub antynagrode za rzut monetą w grze. Za każdym razem gdy rzucisz monetą dostaniesz losowy efekt, zależny od wyniku rzutu monetą.

- Plugin testowany na wersji gry **13.6.9**

## Cechy pluginu:

- Za każdym razem kiedy gracz rzuci monetą jedna z poniższych rzeczy się wydarzy:  
 1. Otrzyma kartę technika zapezbieczeń.  
 2. Otrzyma 'zestaw medyczny' składający sie z apteczki oraz tabletek na ból.
 3. Otrzyma teleport do drzwi strefowych.
 4. Otrzyma leczenie na 25HP.
 5. Jego HP zostanie zwiększone o 10%.
 6. Otrzyma SCP-268.
 7. Otrzyma losowy dobry efekt na 5 sekund.
 8. Otrzyma Logicer'a z 1 ammo.  
 9. Otrzyma SCP-2176. 
 10. Otrzyma różowygo cukierka. 
 11. Otrzyma rewolwer z najgorszymi dodatkami jakie można mieć. 
 12. Otrzyma rozładowanego micro hida.
 13. Natychmiastowy respawn MTF/CI.
 14. Otrzyma zmniejszenie postaci do skali 1.3/0.5/1.3.
 15. Otrzyma losowy item.

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
 12. Fałszywa cassie wysyła komunikat że SCP-173 zostaje zabite przez tesle.
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
  - Decontaminating
  - Disabled
  - Ensnared
  - Exhausted
  - Flashed
  - Hemorrhage
  - Hypothermia
  - InsufficientLighting
  - Poisoned
  - PocketCorroding
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
  - Scp1853
  - Scp207
  - Vitality
  # The % chance of receiving a Facility Manager keycard instead of a Containment Engineer one.
  red_card_chance: 15
  # The kick reason.
  kick_reason: 'Moneta postanowiła wywalić cię z serwera.'
  # The chance of these good effects happening. It's a proportional chance not a % chance.
  keycard_chance: 20
  medical_kit_chance: 30
  tp_to_escape_chance: 5
  heal_chance: 20
  more_hp_chance: 20
  hat_chance: 10
  random_good_effect_chance: 30
  one_ammo_logicer_chance: 5
  lightbulb_chance: 15
  pink_candy_chance: 20
  bad_revo_chance: 5
  empty_hid_chance: 1
  force_respawn_chance: 15
  size_change_chance: 20
  random_item_chance: 35
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
  turn_into_scp_chance: 10
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
  fakentfchance: 5
```
