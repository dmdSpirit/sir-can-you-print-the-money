#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.MovingResources;
using NovemberProject.System.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.CoreGameplay
{
    public sealed class MoneyController
    {
        private readonly ReactiveProperty<int> _governmentMoney = new();
        private readonly ReactiveProperty<int> _armyMoney = new();
        private readonly ReactiveProperty<int> _folkMoney = new();

        private readonly BuildingsController _buildingsController;
        private readonly ResourceMoveEffectSpawner _resourceMoveEffectSpawner;
        private readonly MoneyControllerSettings _settings;
        private readonly MessageBroker _messageBroker;

        public IReadOnlyReactiveProperty<int> GovernmentMoney => _governmentMoney;
        public IReadOnlyReactiveProperty<int> ArmyMoney => _armyMoney;
        public IReadOnlyReactiveProperty<int> FolkMoney => _folkMoney;

        public MoneyController(MoneyControllerSettings moneyControllerSettings, BuildingsController buildingsController,
            ResourceMoveEffectSpawner resourceMoveEffectSpawner, MessageBroker messageBroker)
        {
            _settings = moneyControllerSettings;
            _buildingsController = buildingsController;
            _resourceMoveEffectSpawner = resourceMoveEffectSpawner;
            _messageBroker = messageBroker;
            messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        private void OnNewGame(NewGameMessage message)
        {
            _governmentMoney.Value = _settings.GovernmentMoney;
            _folkMoney.Value = _settings.FolkMoney;
            _armyMoney.Value = _settings.ArmyMoney;
        }

        public void PrintMoney(int money)
        {
            _governmentMoney.Value += money;
        }

        public void BurnMoney(int money)
        {
            int moneyToBurn = Mathf.Min(money, _governmentMoney.Value);
            _governmentMoney.Value -= moneyToBurn;
        }

        public void TransferMoneyFromGovernmentToArmySilent(int money)
        {
            Assert.IsTrue(_governmentMoney.Value >= money);
            _governmentMoney.Value -= money;
            _armyMoney.Value += money;
        }

        public void TransferMoneyFromGovernmentToArmy(int money)
        {
            Assert.IsTrue(_governmentMoney.Value >= money);
            Building governmentTreasury = _buildingsController.GetBuilding(BuildingType.GovernmentTreasury);
            Building armyTreasury = _buildingsController.GetBuilding(BuildingType.ArmyTreasury);
            _governmentMoney.Value -= money;
            ShowCoinMove(governmentTreasury.transform, armyTreasury.transform,
                () => _armyMoney.Value += money);
        }

        public void TransferMoneyFromArmyToFolkSilent(int money)
        {
            Assert.IsTrue(_armyMoney.Value >= money);
            _armyMoney.Value -= money;
            _folkMoney.Value += money;
        }

        public void TransferMoneyFromArmyToFolk(int money)
        {
            Assert.IsTrue(_armyMoney.Value >= money);
            Building armyTreasury = _buildingsController.GetBuilding(BuildingType.ArmyTreasury);
            Building folkTreasury = _buildingsController.GetBuilding(BuildingType.FolkTreasury);
            _armyMoney.Value -= money;
            ShowCoinMove(armyTreasury.transform, folkTreasury.transform, () => _folkMoney.Value += money);
        }

        public void TransferMoneyFromFolkToGovernmentSilent(int money)
        {
            Assert.IsTrue(_folkMoney.Value >= money);
            _governmentMoney.Value += money;
            _folkMoney.Value -= money;
        }

        public void TransferMoneyFromFolkToGovernment(int money)
        {
            Assert.IsTrue(_folkMoney.Value >= money);
            Building folkTreasury = _buildingsController.GetBuilding(BuildingType.FolkTreasury);
            Building governmentTreasury = _buildingsController.GetBuilding(BuildingType.GovernmentTreasury);
            _folkMoney.Value -= money;
            ShowCoinMove(folkTreasury.transform, governmentTreasury.transform,
                () => _governmentMoney.Value += money);
        }

        public void SpedArmyMoney(int money)
        {
            Assert.IsTrue(_armyMoney.Value >= money);
            _armyMoney.Value -= money;
        }

        private void ShowCoinMove(Transform start, Transform finish, Action callback)
        {
            MoveEffect effect = _resourceMoveEffectSpawner.ShowMovingCoin(start.position, finish.position);
            effect.OnFinished.Subscribe(_ => callback.Invoke());
        }
    }
}