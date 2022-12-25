#nullable enable
using UnityEngine;

namespace NovemberProject.Buildings
{
    [CreateAssetMenu(menuName = "Create ResourceProviderSettings", fileName = "ResourceProviderSettings", order = 0)]
    public sealed class ResourceProviderSettings : ScriptableObject
    {
        [SerializeField]
        private Sprite _foodIcon = null!;

        [SerializeField]
        private Sprite _moneyIcon = null!;

        [SerializeField]
        private Sprite _stoneIcon = null!;

        [SerializeField]
        private Sprite _treasureIcon = null!;

        [SerializeField]
        private string _foodTitle = "Food";

        [SerializeField]
        private string _moneyTitle = "Money";

        [SerializeField]
        private string _stoneTitle = "Stone";

        [SerializeField]
        private string _treasureTitle = "Treasure";

        public Sprite FoodIcon => _foodIcon;
        public Sprite MoneyIcon => _moneyIcon;
        public Sprite StoneIcon => _stoneIcon;
        public Sprite TreasureIcon => _treasureIcon;
        public string FoodTitle => _foodTitle;
        public string MoneyTitle => _moneyTitle;
        public string StoneTitle => _stoneTitle;
        public string TreasureTitle => _treasureTitle;
    }
}