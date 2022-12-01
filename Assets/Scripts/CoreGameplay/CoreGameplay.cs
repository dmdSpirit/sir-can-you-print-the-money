#nullable enable
using System.Collections.Generic;
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using NovemberProject.Rounds.UI;
using NovemberProject.System;
using NovemberProject.Time;
using UnityEngine;

namespace NovemberProject.CoreGameplay
{
    [RequireComponent(typeof(FolkManager))]
    [RequireComponent(typeof(ArmyManager))]
    public sealed class CoreGameplay : InitializableBehaviour
    {
        private readonly RoundResult _roundResult = new();
        private readonly List<Timer> _timers = new();

        private GameOverType _gameOverType;

        [SerializeField]
        private int _foodPerPerson = 2;

        [SerializeField]
        private int _newFolkForFoodCost = 10;

        [SerializeField]
        private int _newArmyForFoodCost = 10;

        [SerializeField]
        private float _folkEatTime = 20f;

        [SerializeField]
        private float _folkPayTime = 20f;

        [SerializeField]
        private float _armyEatTime = 20f;

        [SerializeField]
        private float _armyPayTime = 20f;

        public int FoodPerPerson => _foodPerPerson;
        public int NewFolkForFoodCost => _newFolkForFoodCost;
        public int NewArmyForFoodCost => _newArmyForFoodCost;

        public FolkManager FolkManager { get; private set; } = null!;
        public ArmyManager ArmyManager { get; private set; } = null!;
        public GameOverType GameOverType => _gameOverType;
        public RoundResult RoundResult => _roundResult;

        private void Awake()
        {
            FolkManager = GetComponent<FolkManager>();
            ArmyManager = GetComponent<ArmyManager>();
        }

        public void InitializeGameData()
        {
            FolkManager.InitializeGameData();
            ArmyManager.InitializeGameData();
            _gameOverType = GameOverType.None;
        }

        public void StartRound()
        {
            _roundResult.Reset();
            StopTimers();
            StartTimers();
        }

        private void StartTimers()
        {
            TimeSystem timeSystem = Game.Instance.TimeSystem;
            Timer folkEatTimer = timeSystem.CreateTimer(_folkEatTime, OnFolkEat);
            folkEatTimer.Start();
            _timers.Add(folkEatTimer);
            Timer armyEatTimer = timeSystem.CreateTimer(_armyEatTime, OnArmyEat);
            armyEatTimer.Start();
            _timers.Add(armyEatTimer);
            Timer folkPayTimer = timeSystem.CreateTimer(_folkPayTime, OnFolkPay);
            folkPayTimer.Start();
            _timers.Add(folkPayTimer);
            Timer armyPayTimer = timeSystem.CreateTimer(_armyPayTime, OnArmyPay);
            armyPayTimer.Start();
            _timers.Add(armyPayTimer);
            folkEatTimer.Start();
        }

        private void OnFolkEat(Timer timer)
        {
            _timers.Remove(timer);
            Game.Instance.FolkManager.EatFood();
        }

        private void OnArmyEat(Timer timer)
        {
            _timers.Remove(timer);
            Game.Instance.ArmyManager.EatFood();
        }

        private void OnFolkPay(Timer timer)
        {
            _timers.Remove(timer);
            Game.Instance.FolkManager.PayTaxes();
        }

        private void OnArmyPay(Timer timer)
        {
            _timers.Remove(timer);
            Game.Instance.ArmyManager.PaySalary();
        }

        private void StopTimers()
        {
            foreach (Timer timer in _timers)
            {
                timer.Cancel();
            }

            _timers.Clear();
        }

        public void EndRound()
        {
        }

        public bool IsGameOver()
        {
            if (IsNoArmyLeft())
            {
                _gameOverType = GameOverType.NoArmy;
                StopTimers();
                return true;
            }

            if (!IsNoFolkLeft())
            {
                return false;
            }

            _gameOverType = GameOverType.NoFolk;
            StopTimers();
            return true;
        }

        public void OnFolkStarved(int count)
        {
            if (count > 0)
            {
                Game.Instance.UIManager.ShowNotification(NotificationType.FolkStarved, count);
            }

            _roundResult.FolkStarved += count;
        }

        public void OnArmyStarved(int count)
        {
            if (count > 0)
            {
                Game.Instance.UIManager.ShowNotification(NotificationType.ArmyStarved, count);
            }

            _roundResult.ArmyStarved += count;
        }

        public void OnFolkExecuted(int count)
        {
            if (count > 0)
            {
                Game.Instance.UIManager.ShowNotification(NotificationType.FolkExecuted, count);
            }

            _roundResult.FolkExecuted += count;
        }

        public void OnArmyDeserted(int count)
        {
            if (count > 0)
            {
                Game.Instance.UIManager.ShowNotification(NotificationType.ArmyDeserted, count);
            }

            _roundResult.ArmyDeserted += count;
        }

        private bool IsNoFolkLeft()
        {
            FoodController foodController = Game.Instance.FoodController;
            var folkCount = FolkManager.FolkCount;
            return folkCount.Value == 0
                   && foodController.FolkFood.Value < _newFolkForFoodCost;
        }

        private bool IsNoArmyLeft()
        {
            FoodController foodController = Game.Instance.FoodController;
            var armyCount = ArmyManager.GuardsCount;
            return armyCount.Value == 0
                   && ArmyManager.ExplorersCount.Value == 0
                   && foodController.ArmyFood.Value < _newArmyForFoodCost;
        }
    }
}