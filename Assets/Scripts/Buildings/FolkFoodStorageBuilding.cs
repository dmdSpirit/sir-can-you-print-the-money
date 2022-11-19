#nullable enable
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class FolkFoodStorageBuilding : Building
    {
        [SerializeField]
        private TMP_Text _foodText = null!;

        public override BuildingType BuildingType => BuildingType.FolkFoodStorage;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.FoodController.FolkFood
                .TakeUntilDisable(this)
                .Subscribe(OnFoodChanged);
        }

        private void OnFoodChanged(int money)
        {
            _foodText.text = money.ToString();
        }
    }
}