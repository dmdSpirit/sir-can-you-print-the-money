#nullable enable
using UnityEngine;

namespace NovemberProject.CoreGameplay.FolkManagement
{
    [CreateAssetMenu(menuName = "Create FolkManagerSettings", fileName = "FolkManagerSettings", order = 0)]
    public sealed class FolkManagerSettings : ScriptableObject
    {
        [SerializeField]
        private int _tax = 6;

        [SerializeField]
        private int _farmWorkers = 1;

        [SerializeField]
        private int _marketWorkers = 1;

        [SerializeField]
        private int _mineWorkers = 0;

        [SerializeField]
        private int _maxMarketWorkers = 5;

        [SerializeField]
        private int _newUnitFoodCost = 5;

        [SerializeField]
        private int _foodUpkeep = 2;

        public int FolkTax => _tax;
        public int FarmWorkers => _farmWorkers;
        public int MarketWorkers => _marketWorkers;
        public int MineWorkers => _mineWorkers;
        public int MaxMarketWorkers => _maxMarketWorkers;
        public int NewUnitFoodCost => _newUnitFoodCost;
        public int FoodUpkeep => _foodUpkeep;
    }
}