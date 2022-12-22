#nullable enable
using NovemberProject.System;
using NovemberProject.Treasures;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class TreasureTowerBuilding : Building, IResourceStorage
    {
        private TreasureController _treasureController = null!;

        [SerializeField]
        private Sprite _treasureIcon = null!;

        [SerializeField]
        private TMP_Text _treasureCountText = null!;

        [SerializeField]
        private string _treasureTitle = "Treasures";

        public override BuildingType BuildingType => BuildingType.TreasureTower;
        public Sprite SpriteIcon => _treasureIcon;
        public IReadOnlyReactiveProperty<int> ResourceCount => _treasureController.Treasures;
        public string ResourceTitle => _treasureTitle;

        [Inject]
        private void Construct(TreasureController treasureController)
        {
            _treasureController = treasureController;
            _treasureController.Treasures.Subscribe(OnTreasureCountChanged);
        }

        private void OnTreasureCountChanged(int treasures)
        {
            _treasureCountText.text = treasures.ToString();
        }
    }
}