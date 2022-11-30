#nullable enable
using NovemberProject.Buildings;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.CoreGameplay
{
    public sealed class Expeditions : InitializableBehaviour
    {
        private Timer? _expeditionTimer;
        private readonly ReactiveProperty<bool> _isExpeditionActive = new();
        private int _explorersLeftForExpedition;
        private int _expeditionIndex;

        [SerializeField]
        private int _expeditionDuration = 20;

        [SerializeField]
        private ExpeditionData[] _expeditionDatas = null!;

        public IReadOnlyReactiveProperty<bool> IsExpeditionActive => _isExpeditionActive;
        public IReadOnlyTimer? Timer => _expeditionTimer;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.MessageBroker.Receive<NewGameMessage>()
                .TakeUntilDisable(this)
                .Subscribe(OnNewGame);
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
            int maxIndex = _expeditionDatas.Length - 1;
            if (_expeditionIndex < maxIndex)
            {
                return _expeditionDatas[_expeditionIndex];
            }

            return _expeditionDatas[maxIndex];
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
            var expeditionsBuilding = Game.Instance.BuildingsController.GetBuilding<ExpeditionsBuilding>();
            _explorersLeftForExpedition = Game.Instance.ArmyManager.ExplorersCount.Value;
            PayFood(expeditionsBuilding, _explorersLeftForExpedition);
            PayMoney(expeditionsBuilding, _explorersLeftForExpedition);
            _expeditionTimer = Game.Instance.TimeSystem.CreateTimer(_expeditionDuration, OnExpeditionFinished);
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

            FoodController foodController = Game.Instance.FoodController;
            foodController.SpendArmyFood(foodCost);
        }

        private void PayMoney(ExpeditionsBuilding building, int explorers)
        {
            int moneyCost = building.ExpeditionMoneyPerPersonCost * explorers;
            if (moneyCost == 0)
            {
                return;
            }

            MoneyController moneyController = Game.Instance.MoneyController;
            moneyController.SpedArmyMoney(moneyCost);
        }

        private void OnExpeditionFinished(Timer timer)
        {
            _isExpeditionActive.Value = false;
            bool isSuccess = RollExpeditionResult();
            int reward = isSuccess ? GetCurrentExpeditionData().Reward : 0;
            var expeditionResult = new ExpeditionResult(_explorersLeftForExpedition, reward, isSuccess, GetCurrentExpeditionData().Defenders);
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