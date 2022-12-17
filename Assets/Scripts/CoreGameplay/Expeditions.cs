#nullable enable
using NovemberProject.Buildings;
using NovemberProject.System;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.CoreGameplay
{
    public sealed class Expeditions
    {
        private readonly ReactiveProperty<bool> _isExpeditionActive = new();

        private readonly FoodController _foodController;
        private readonly MoneyController _moneyController;
        private readonly BuildingsController _buildingsController;
        private readonly MessageBroker _messageBroker;
        private readonly ExpeditionSettings _settings;

        private Timer? _expeditionTimer;
        private int _explorersLeftForExpedition;
        private int _expeditionIndex;

        public IReadOnlyReactiveProperty<bool> IsExpeditionActive => _isExpeditionActive;
        public IReadOnlyTimer? Timer => _expeditionTimer;

        public Expeditions(ExpeditionSettings expeditionSettings, FoodController foodController,
            MoneyController moneyController,
            BuildingsController buildingsController,
            MessageBroker messageBroker)
        {
            _settings = expeditionSettings;
            _foodController = foodController;
            _moneyController = moneyController;
            _buildingsController = buildingsController;
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        public float GetExpeditionWinProbability()
        {
            ExpeditionData data = GetCurrentExpeditionData();
            return Game.Instance.CombatController.GetAttackersWinProbability(
                Game.Instance.ArmyManager.ExplorersCount.Value,
                data.Defenders);
        }

        public ExpeditionData GetCurrentExpeditionData()
        {
            int maxIndex = _settings.ExpeditionDatas.Count - 1;
            if (_expeditionIndex < maxIndex)
            {
                return _settings.ExpeditionDatas[_expeditionIndex];
            }

            return _settings.ExpeditionDatas[maxIndex];
        }

        private bool RollExpeditionResult()
        {
            float probability = GetExpeditionWinProbability();
            float roll = Random.value;
            return roll <= probability;
        }

        public void StartExpedition()
        {
            Assert.IsFalse(_isExpeditionActive.Value);
            var expeditionsBuilding = _buildingsController.GetBuilding<ExpeditionsBuilding>();
            _explorersLeftForExpedition = Game.Instance.ArmyManager.ExplorersCount.Value;
            PayFood(expeditionsBuilding, _explorersLeftForExpedition);
            PayMoney(expeditionsBuilding, _explorersLeftForExpedition);
            _expeditionTimer = Game.Instance.TimeSystem.CreateTimer(_settings.ExpeditionDuration, OnExpeditionFinished);
            _expeditionTimer.Start();
            _isExpeditionActive.Value = true;
        }

        private void PayFood(ExpeditionsBuilding building, int explorers)
        {
            int foodCost = building.ExpeditionFoodPerPersonCost * explorers;
            if (foodCost == 0)
            {
                return;
            }

            _foodController.SpendArmyFood(foodCost);
        }

        private void PayMoney(ExpeditionsBuilding building, int explorers)
        {
            int moneyCost = building.ExpeditionMoneyPerPersonCost * explorers;
            if (moneyCost == 0)
            {
                return;
            }

            _moneyController.SpedArmyMoney(moneyCost);
        }

        private void OnExpeditionFinished(Timer timer)
        {
            _isExpeditionActive.Value = false;
            bool isSuccess = RollExpeditionResult();
            int reward = isSuccess ? GetCurrentExpeditionData().Reward : 0;
            var expeditionResult = new ExpeditionResult(_explorersLeftForExpedition, reward, isSuccess,
                GetCurrentExpeditionData().Defenders);
            Game.Instance.GameStateMachine.ExpeditionFinished(expeditionResult);
            if (isSuccess)
            {
                Game.Instance.TreasureController.AddTreasures(reward);
                _expeditionIndex++;
            }
        }

        private void OnNewGame(NewGameMessage _)
        {
            _isExpeditionActive.Value = false;
            _expeditionTimer?.Cancel();
            _expeditionIndex = 0;
        }
    }
}