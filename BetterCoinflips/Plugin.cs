﻿using Exiled.API.Features;
using System;
using BetterCoinflips.Configs;
using Map = Exiled.Events.Handlers.Map;
using Player = Exiled.Events.Handlers.Player;

namespace BetterCoinflips
{
    public class Plugin : Plugin<Config, Configs.Translations>
    {
        public override Version RequiredExiledVersion => new(8, 7, 0, 0);
        public override Version Version => new(6, 1, 0);
        public override string Author => "Miki_hero - fork(snowycoldd)";
        public override string Name => "BetterCoinflips";

        public static Plugin Instance;
        private EventHandlers _eventHandler;
        
        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnRegisterEvents();
            Instance = null;
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _eventHandler = new EventHandlers();
            Player.FlippingCoin += _eventHandler.OnCoinFlip;
            Map.SpawningItem += _eventHandler.OnSpawningItem;
            Map.FillingLocker += _eventHandler.OnFillingLocker;
            Player.Died += _eventHandler.OnPlayerDied;
            Player.Spawning += _eventHandler.OnPlayerSpawned;
        }

        private void UnRegisterEvents()
        {
            Player.FlippingCoin -= _eventHandler.OnCoinFlip;
            Map.SpawningItem -= _eventHandler.OnSpawningItem;
            Map.FillingLocker -= _eventHandler.OnFillingLocker;
            Player.Died -= _eventHandler.OnPlayerDied;
            Player.Spawning -= _eventHandler.OnPlayerSpawned;
            _eventHandler = null;
        }
    }
}