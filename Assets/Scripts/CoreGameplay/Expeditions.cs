#nullable enable
using NovemberProject.Buildings;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
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

        [SerializeField]
        private int _expeditionDuration = 20;

        [SerializeField]
        private int _expeditionReward = 1;

        public IReadOnlyReactiveProperty<bool> IsExpeditionActive => _isExpeditionActive;
        public IReadOnlyTimer? Timer => _expeditionTimer;

        public int ExpeditionReward => _expeditionReward;

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
            var expeditionResult = new ExpeditionResult(_explorersLeftForExpedition, _expeditionReward);
            Game.Instance.GameStateMachine.ExpeditionFinished(expeditionResult);
        }
    }
}