#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.Money;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Buildings
{
    public sealed class FoodController : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _armyFood = new();
        private readonly ReactiveProperty<int> _folkFood = new();

        [SerializeField]
        private int _startingArmyFood = 30;

        [SerializeField]
        private int _startingFolkFood = 30;

        public IReadOnlyReactiveProperty<int> ArmyFood => _armyFood;
        public IReadOnlyReactiveProperty<int> FolkFood => _folkFood;

        public void InitializeGameData()
        {
            _armyFood.Value = _startingArmyFood;
            _folkFood.Value = _startingFolkFood;
        }

        public void SpendArmyFood(int food)
        {
            Assert.IsTrue(_armyFood.Value >= food);
            _armyFood.Value -= food;
        }

        public void SpendFolkFood(int food)
        {
            Assert.IsTrue(_folkFood.Value >= food);
            _folkFood.Value -= food;
        }

        public void TransferFoodFromFolkToArmy(int food)
        {
            Assert.IsTrue(_folkFood.Value >= food);
            BuildingsController buildingsController = Game.Instance.BuildingsController;
            Building folkFoodStorage = buildingsController.GetBuilding(BuildingType.FolkFoodStorage);
            Building armyFoodStorage = buildingsController.GetBuilding(BuildingType.ArmyFoodStorage);
            ShowFoodMove(folkFoodStorage.transform, armyFoodStorage.transform, () =>
            {
                _folkFood.Value -= food;
                _armyFood.Value += food;
            });
        }

        public void ProduceFoodFromFarm(int food)
        {
            BuildingsController buildingsController = Game.Instance.BuildingsController;
            Building farm = buildingsController.GetBuilding(BuildingType.Farm);
            Building folkFoodStorage = buildingsController.GetBuilding(BuildingType.FolkFoodStorage);
            ShowFoodMove(farm.transform, folkFoodStorage.transform, () => _folkFood.Value += food);
        }

        private void ShowFoodMove(Transform start, Transform finish, Action callback)
        {
            ResourceMoveEffectSpawner moveEffectSpawner = Game.Instance.ResourceMoveEffectSpawner;
            MoveEffect effect = moveEffectSpawner.ShowMovingFood(start.position, finish.position);
            effect.OnFinished.Subscribe(_ => callback.Invoke());
        }
    }
}