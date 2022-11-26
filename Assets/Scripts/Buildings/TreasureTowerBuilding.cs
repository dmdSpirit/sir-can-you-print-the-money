#nullable enable
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class TreasureTowerBuilding : Building, IResourceStorage
    {
        [SerializeField]
        private Sprite _treasureIcon = null!;

        [SerializeField]
        private TMP_Text _treasureCountText = null!;

        [SerializeField]
        private string _treasureTitle = "Treasures";
        
        public override BuildingType BuildingType => BuildingType.TreasureTower;
        public Sprite SpriteIcon => _treasureIcon;
        public IReadOnlyReactiveProperty<int> ResourceCount => Game.Instance.TreasureController.Treasures;
        public string ResourceTitle => _treasureTitle;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.TreasureController.Treasures
                .TakeUntilDisable(this)
                .Subscribe(OnTreasureCountChanged);
        }

        private void OnTreasureCountChanged(int treasures)
        {
            _treasureCountText.text = treasures.ToString();
        }
    }
}