#nullable enable
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class ArmyFoodStorageBuilding : Building, IResourceStorage, IBuyUnit
    {
        [SerializeField]
        private TMP_Text _foodText = null!;
        
        [SerializeField]
        private Sprite _foodSprite = null!;

        [SerializeField]
        private string _foodTitle = "Food";
        
        [SerializeField]
        private string _buyUnitTitle = "Buy an army";

        [SerializeField]
        private string _buyUnitButtonText = "Buy";

        public override BuildingType BuildingType => BuildingType.ArmyFoodStorage;
        public Sprite SpriteIcon => _foodSprite;
        public string ResourceTitle => _foodTitle;
        public IReadOnlyReactiveProperty<int> ResourceCount => Game.Instance.FoodController.ArmyFood;
        public bool CanBuyUnit =>
            Game.Instance.FoodController.ArmyFood.Value >= Game.Instance.CoreGameplay.NewArmyForFoodCost;

        public string BuyUnitTitle => _buyUnitTitle;
        public string BuyUnitButtonText => _buyUnitButtonText;


        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.FoodController.ArmyFood
                .TakeUntilDisable(this)
                .Subscribe(OnFoodChanged);
        }
        
        public void BuyUnit()
        {
            if (!CanBuyUnit)
            {
                return;
            }
            Game.Instance.ArmyManager.BuyArmyForFood();
        }

        private void OnFoodChanged(int food)
        {
            _foodText.text = food.ToString();
        }
    }
}