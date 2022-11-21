#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using NovemberProject.Rounds.UI;
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.CoreGameplay
{
    [RequireComponent(typeof(FolkManager))]
    [RequireComponent(typeof(ArmyManager))]
    public sealed class CoreGameplay : InitializableBehaviour
    {
        private GameOverType _gameOverType;

        [SerializeField]
        private int _foodPerPerson = 2;

        [SerializeField]
        private int _newFolkForFoodCost = 10;

        [SerializeField]
        private int _newArmyForFoodCost = 10;

        public int FoodPerPerson => _foodPerPerson;
        public int NewFolkForFoodCost => _newFolkForFoodCost;
        public int NewArmyForFoodCost => _newArmyForFoodCost;

        public FolkManager FolkManager { get; private set; } = null!;
        public ArmyManager ArmyManager { get; private set; } = null!;
        public GameOverType GameOverType => _gameOverType;

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
            FolkManager.StartRound();
            ArmyManager.StartRound();
        }

        public void EndRound()
        {
            FolkManager.EndRound();
            ArmyManager.EndRound();
        }

        public bool IsGameOver()
        {
            if (IsNoArmyLeft())
            {
                _gameOverType = GameOverType.NoArmy;
                return true;
            }

            if (!IsNoFolkLeft())
            {
                return false;
            }

            _gameOverType = GameOverType.NoFolk;
            return true;
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
            var armyCount = ArmyManager.ArmyCount;
            return armyCount.Value == 0
                   && foodController.ArmyFood.Value < _newArmyForFoodCost;
        }
    }
}