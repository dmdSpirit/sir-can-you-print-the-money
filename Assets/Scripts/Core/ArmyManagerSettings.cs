#nullable enable
using UnityEngine;

namespace NovemberProject.Core
{
    [CreateAssetMenu(menuName = "Create ArmyManagerSettings", fileName = "ArmyManagerSettings", order = 0)]
    public sealed class ArmyManagerSettings : ScriptableObject
    {
        [SerializeField]
        private int _armySalary = 8;

        [SerializeField]
        private int _guardsCount = 2;

        [SerializeField]
        private int _explorersCount = 0;

        [SerializeField]
        private int _newUnitFoodCost = 5;

        [SerializeField]
        private int _foodUpkeep = 2;

        public int ArmySalary => _armySalary;
        public int GuardsCount => _guardsCount;
        public int ExplorersCount => _explorersCount;
        public int NewUnitFoodCost => _newUnitFoodCost;
        public int FoodUpkeep => _foodUpkeep;
    }
}