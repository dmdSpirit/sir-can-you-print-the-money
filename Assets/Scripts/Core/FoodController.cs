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
    public sealed class FoodController
    {
        private readonly ReactiveProperty<int> _armyFood = new();
        private readonly ReactiveProperty<int> _folkFood = new();

        private readonly BuildingsController _buildingsController;
        private readonly ResourceMoveEffectSpawner _resourceMoveEffectSpawner;
        private readonly MessageBroker _messageBroker;
        private readonly FoodControllerSettings _settings;

        public IReadOnlyReactiveProperty<int> ArmyFood => _armyFood;
        public IReadOnlyReactiveProperty<int> FolkFood => _folkFood;

        public FoodController(FoodControllerSettings foodControllerSettings, BuildingsController buildingsController,
            ResourceMoveEffectSpawner resourceMoveEffectSpawner, MessageBroker messageBroker)
        {
            _settings = foodControllerSettings;
            _buildingsController = buildingsController;
            _resourceMoveEffectSpawner = resourceMoveEffectSpawner;
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        private void OnNewGame(NewGameMessage message)
        {
            _armyFood.Value = _settings.StartingArmyFood;
            _folkFood.Value = _settings.StartingFolkFood;
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
            Building folkFoodStorage = _buildingsController.GetBuilding(BuildingType.FolkFoodStorage);
            Building armyFoodStorage = _buildingsController.GetBuilding(BuildingType.ArmyFoodStorage);
            ShowFoodMove(folkFoodStorage.transform, armyFoodStorage.transform, () =>
            {
                _folkFood.Value -= food;
                _armyFood.Value += food;
            });
        }

        public void ProduceFoodFromFarm(int food)
        {
            Building farm = _buildingsController.GetBuilding(BuildingType.Farm);
            Building folkFoodStorage = _buildingsController.GetBuilding(BuildingType.FolkFoodStorage);
            ShowFoodMove(farm.transform, folkFoodStorage.transform, () => _folkFood.Value += food);
        }

        private void ShowFoodMove(Transform start, Transform finish, Action callback)
        {
            MoveEffect effect = _resourceMoveEffectSpawner.ShowMovingFood(start.position, finish.position);
            effect.OnFinished.Subscribe(_ => callback.Invoke());
        }
    }
}