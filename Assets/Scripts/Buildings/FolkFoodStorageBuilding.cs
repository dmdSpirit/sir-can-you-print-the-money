#nullable enable
using System;
using NovemberProject.CoreGameplay;
using NovemberProject.CoreGameplay.FolkManagement;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class FolkFoodStorageBuilding : Building, IResourceStorage, IBuyUnit
    {
        private FolkManager _folkManager = null!;
        private FoodController _foodController = null!;
        
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
        public IReadOnlyReactiveProperty<int> ResourceCount => _foodController.FolkFood;

        public bool CanBuyUnit => _folkManager.IsEnoughFoodForNewFolk();

        public string BuyUnitTitle => _buyUnitTitle;
        public string BuyUnitButtonText => _buyUnitButtonText;
        
        [Inject]
        private void Construct(FolkManager folkManager, FoodController foodController)
        {
            _folkManager = folkManager;
            _foodController = foodController;
            _foodController.FolkFood.Subscribe(OnFoodChanged);
        }

        public void BuyUnit()
        {
            if (!CanBuyUnit)
            {
                return;
            }
            _folkManager.BuyFolkForFood();
        }

        private void OnFoodChanged(int money)
        {
            _foodText.text = money.ToString();
        }
    }
}