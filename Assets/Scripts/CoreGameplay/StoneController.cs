#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.CommonUIStuff;
using NovemberProject.MovingResources;
using NovemberProject.System;
using NovemberProject.System.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace NovemberProject.CoreGameplay
{
    public sealed class StoneController : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _stone = new();

        private BuildingsController _buildingsController = null!;
        private MessageBroker _messageBroker = null!;

        [SerializeField]
        private int _startingStone = 0;

        public IReadOnlyReactiveProperty<int> Stone => _stone;

        [Inject]
        private void Construct(BuildingsController buildingsController, MessageBroker messageBroker)
        {
            _buildingsController = buildingsController;
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        private void OnNewGame(NewGameMessage message)
        {
            _stone.Value = _startingStone;
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
            ResourceMoveEffectSpawner moveEffectSpawner = Game.Instance.ResourceMoveEffectSpawner;
            MoveEffect effect = moveEffectSpawner.ShowMovingCoin(start.position, finish.position);
            effect.OnFinished.Subscribe(_ => callback.Invoke());
        }
    }
}