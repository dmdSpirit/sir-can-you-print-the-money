#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class ArmyFoodStorageBuilding : Building, IResourceStorage, IBuyUnit
    {
        private FoodController _foodController = null!;

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
        public IReadOnlyReactiveProperty<int> ResourceCount => _foodController.ArmyFood;

        public bool CanBuyUnit =>
            _foodController.ArmyFood.Value >= Game.Instance.CoreGameplay.NewArmyForFoodCost;

        public string BuyUnitTitle => _buyUnitTitle;
        public string BuyUnitButtonText => _buyUnitButtonText;

        [Inject]
        private void Construct(FoodController foodController)
        {
            _foodController = foodController;
            _foodController.ArmyFood.Subscribe(OnFoodChanged);
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