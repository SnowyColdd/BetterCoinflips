<a href="https://github.com/snowycoldd/BetterCoinflips/releases"><img src="https://img.shields.io/github/downloads/snowycoldd/BetterCoinflips/total?label=Downloads" alt="Downloads"></a>  

> For the Polish version of this README, see [README_PL](./README.pl.md)

# Fork  
This plugin is a fork of [BetterCoinflips](https://github.com/Mikihero/BetterCoinflips) by [Mikihero](https://github.com/Mikihero), rewritten in Polish and enhanced with new features.

## BetterCoinflips  

A plugin for SCP:SL that adds rewards or penalties based on a coin flip in-game. Every time you flip a coin, you'll get a random effect depending on the outcome.

- Plugin tested on game version **14.1 beta**

## Plugin Features

- Each time a player flips a coin, one of the following rewards may occur:
1. Receive a random keycard.  
2. Receive a "medical kit" (medkit + painkillers).  
3. Get teleported to zone doors.  
4. Get fully healed.  
5. HP increases by 10%.  
6. Receive a random SCP item.  
7. Gain a random good effect for 5 seconds.  
8. Gain wallhack for 15 seconds.  
9. Receive a pink candy.  
10. Receive a revolver with the worst possible attachments.  
11. Receive a charged micro HID.  
12. Instant MTF/CI respawn.  
13. Get resized to 1.3/0.5/1.3 scale.  
14. Receive a random item.  
15. Gain temporary invincibility for 5 seconds.  
16. Upgrade held keycard to a higher level.  
17. Refill all ammo.  
18. Receive a random amount of HP.  
19. Gain 1000 HP for a limited time.  
20. Activate a random generator.  
21. Domino effect – nearby players receive random positive effects.  
22. Time loop – teleports players back to their positions after 10 seconds.

- If the coin lands on tails, one of the following punishments may occur:
1. HP reduced by 30%.  
2. Teleported to D-Class cells.  
3. Receive a random bad effect for 5 seconds.  
4. Alpha Warhead is toggled.  
5. Global lights turned off for 10 seconds.  
6. Live grenade appears overhead.  
7. Live flashbang appears overhead.  
8. Teleported to a live SCP or lose 15HP.  
9. Inventory wiped, 1HP left.  
10. SCP-244 appears under feet.  
11. Gain SCP-173 rage.  
12. Fake Cassie announces SCP death by tesla.  
13. Become a random SCP.  
14. Inventory reset.  
15. Team switch (e.g., D-Class ↔ Scientist, MTF ↔ CI).  
16. Instant explosion grenade on head.  
17. Swap position with another player.  
18. Kicked from the server.  
19. Swapped with a spectator.  
20. Teleported to a random tesla.  
21. Swap inventory with another player.  
22. Teleported to a random room.  
23. Handcuffed and inventory cleared.  
24. Fake MTF arrival announcement.  
25. All players in Light Zone receive decontamination effect.  
26. Random teleport every 5 seconds for 20 seconds.  
27. Flipped upside down for 30 seconds.  
28. All doors in current zone locked for 10 seconds.  
29. Drops random inventory items.  
30. Becomes a walking time bomb.

- Plugin limits the number of coins that spawn on the map.  
- Plugin replaces specific items (default: SCP-500) with coins.  
- Each coin has a randomized number of uses. When it runs out, it breaks.

## Commands

- `GetSerial` – Get serial number of the held item or another player's.  
- `CoinUses` – Get or set number of uses for a coin. Example: `coinuses get player 5`, `coinuses set player 4`, `coinuses set serial 10`  

## Permissions

- `bc.coinuses.set` – Allows use of the `set` subcommand for CoinUses.  
- `bc.coinuses.get` – Allows use of the `get` subcommand for CoinUses.  

## Default Config

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