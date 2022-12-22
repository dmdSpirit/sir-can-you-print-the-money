#nullable enable
using UnityEngine;

namespace NovemberProject.CoreGameplay
{
    [CreateAssetMenu(menuName = "Create FoodControllerSettings", fileName = "FoodControllerSettings", order = 0)]
    public sealed class FoodControllerSettings : ScriptableObject
    {
        [SerializeField]
        private int _startingArmyFood = 15;

        [SerializeField]
        private int _startingFolkFood = 13;

        public int StartingArmyFood => _startingArmyFood;
        public int StartingFolkFood => _startingFolkFood;
    }
}