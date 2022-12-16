#nullable enable
using UnityEngine;

namespace NovemberProject.CoreGameplay.FolkManagement
{
    [CreateAssetMenu(menuName = "Create FolkManagerSettings", fileName = "FolkManagerSettings", order = 0)]
    public sealed class FolkManagerSettings : ScriptableObject
    {
        [SerializeField]
        private int _startingTax = 6;

        [SerializeField]
        private int _startingFarmWorkers = 1;

        [SerializeField]
        private int _startingMarketWorkers = 1;

        [SerializeField]
        private int _startingMineWorkers = 0;

        [SerializeField]
        private int _maxMarketWorkers = 5;

        [SerializeField]
        private int _newUnitFoodCost = 5;

        [SerializeField]
        private int _foodUpkeep = 2;

        public int StartingFolkTax => _startingTax;
        public int StartingFarmWorkers => _startingFarmWorkers;
        public int StartingMarketWorkers => _startingMarketWorkers;
        public int StartingMineWorkers => _startingMineWorkers;
        public int MaxMarketWorkers => _maxMarketWorkers;
        public int NewUnitFoodCost => _newUnitFoodCost;
        public int FoodUpkeep => _foodUpkeep;
    }
}