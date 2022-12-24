#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.MovingResources;
using NovemberProject.System.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Core
{
    public sealed class StoneController
    {
        private readonly ReactiveProperty<int> _stone = new();

        private readonly StoneControllerSettings _settings;
        private readonly BuildingsController _buildingsController;
        private readonly ResourceMoveEffectSpawner _resourceMoveEffectSpawner;
        private readonly MessageBroker _messageBroker;

        public IReadOnlyReactiveProperty<int> Stone => _stone;

        public StoneController(StoneControllerSettings stoneControllerSettings, BuildingsController buildingsController,
            ResourceMoveEffectSpawner resourceMoveEffectSpawner, MessageBroker messageBroker)
        {
            _settings = stoneControllerSettings;
            _buildingsController = buildingsController;
            _resourceMoveEffectSpawner = resourceMoveEffectSpawner;
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        private void OnNewGame(NewGameMessage message)
        {
            _stone.Value = _settings.StatingStone;
        }

        public void AddStone(int stone)
        {
            _stone.Value += stone;
        }

        public void SpendStone(int stone)
        {
            Assert.IsTrue(_stone.Value >= stone);
            _stone.Value -= stone;
        }

        public void MineStone(int stone)
        {
            Building mine = _buildingsController.GetBuilding<MineBuilding>();
            Building stoneStorage = _buildingsController.GetBuilding<ArenaBuilding>();
            ShowStoneMove(mine.transform, stoneStorage.transform,
                () => AddStone(stone));
        }

        private void ShowStoneMove(Transform start, Transform finish, Action callback)
        {
            MoveEffect effect = _resourceMoveEffectSpawner.ShowMovingCoin(start.position, finish.position);
            effect.OnFinished.Subscribe(_ => callback.Invoke());
        }
    }
}