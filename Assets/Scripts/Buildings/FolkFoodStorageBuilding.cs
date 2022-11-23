#nullable enable
using System;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class FolkFoodStorageBuilding : Building, IResourceStorage, IBuyUnit
    {
        [SerializeField]
        private TMP_Text _foodText = null!;

        [SerializeField]
        private Sprite _foodSprite = null!;

        [SerializeField]
        private string _foodTitle = "Food";

        [SerializeField]
        private string _buyUnitTitle = "Buy a folk";

        [SerializeField]
        private string _buyUnitButtonText = "Buy";

        public override BuildingType BuildingType => BuildingType.FolkFoodStorage;
        public Sprite SpriteIcon => _foodSprite;
        public string ResourceTitle => _foodTitle;
        public IReadOnlyReactiveProperty<int> ResourceCount => Game.Instance.FoodController.FolkFood;

        public bool CanBuyUnit =>
            Game.Instance.FoodController.FolkFood.Value >= Game.Instance.CoreGameplay.NewFolkForFoodCost;

        public string BuyUnitTitle => _buyUnitTitle;
        public string BuyUnitButtonText => _buyUnitButtonText;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.FoodController.FolkFood
                .TakeUntilDisable(this)
                .Subscribe(OnFoodChanged);
        }

        public void BuyUnit()
        {
            if (!CanBuyUnit)
            {
                return;
            }
            Game.Instance.FolkManager.BuyFolkForFood();
        }

        private void OnFoodChanged(int money)
        {
            _foodText.text = money.ToString();
        }
    }
}