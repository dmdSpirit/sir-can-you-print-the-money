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
        private string _foodTitle = "Food";

        [SerializeField]
        private string _moneyTitle = "Money";

        public Sprite FoodIcon => _foodIcon;
        public Sprite MoneyIcon => _moneyIcon;
        public string FoodTitle => _foodTitle;
        public string MoneyTitle => _moneyTitle;
    }
}