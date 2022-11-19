#nullable enable
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class FarmBuilding : Building
    {
        private FolkManager _folkManager = null!;

        [SerializeField]
        private TMP_Text _numberOfWorkersText = null!;

        public override BuildingType BuildingType => BuildingType.Farm;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _folkManager = Game.Instance.CoreGameplay.FolkManager;
            _folkManager.FarmFolk
                .TakeUntilDisable(this)
                .Subscribe(OnFarmCountChanged);
        }

        private void OnFarmCountChanged(int farmWorkers)
        {
            _numberOfWorkersText.text = farmWorkers.ToString();
        }

        private void AddFarmWorker()
        {
            _folkManager.AddFolkToFarm();
        }

        private void RemoveFarmWorker()
        {
            _folkManager.RemoveFolkFromFarm();
        }
    }
}