#nullable enable
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class ArmyFoodStorageBuilding : Building, IResourceStorage
    {
        [SerializeField]
        private TMP_Text _foodText = null!;
        
        [SerializeField]
        private Sprite _foodSprite = null!;

        [SerializeField]
        private string _foodTitle = "Food";


        public override BuildingType BuildingType => BuildingType.ArmyFoodStorage;
        public Sprite SpriteIcon => _foodSprite;
        public string ResourceTitle => _foodTitle;
        public IReadOnlyReactiveProperty<int> ResourceCount => Game.Instance.FoodController.ArmyFood;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.FoodController.ArmyFood
                .TakeUntilDisable(this)
                .Subscribe(OnFoodChanged);
        }

        private void OnFoodChanged(int money)
        {
            _foodText.text = money.ToString();
        }
    }
}