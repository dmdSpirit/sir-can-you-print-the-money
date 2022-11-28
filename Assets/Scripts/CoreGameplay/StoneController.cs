#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.CommonUIStuff;
using NovemberProject.MovingResources;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.CoreGameplay
{
    public sealed class StoneController : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _stone = new();

        [SerializeField]
        private int _startingStone = 0;

        public IReadOnlyReactiveProperty<int> Stone => _stone;

        public void InitializeGameData()
        {
            _stone.Value = _startingStone;
        }

        public void AddStone(int stone)
        {
            _stone.Value += stone;
        }
        
        public void SpendStone(int stone)
        {
            Assert.IsTrue(_stone.Value>=stone);
            _stone.Value-=stone;
        }

        public void MineStone(int stone)
        {
            BuildingsController buildingController = Game.Instance.BuildingsController;
            Building mine = buildingController.GetBuilding<MineBuilding>();
            Building stoneStorage = buildingController.GetBuilding<ArenaBuilding>();
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