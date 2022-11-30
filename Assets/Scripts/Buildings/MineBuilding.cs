#nullable enable
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class MineBuilding : Building, IMineWorkerManipulator
    {
        [SerializeField]
        private string _minersTitle = "Miners";

        [SerializeField]
        private TMP_Text _minersCount = null!;
        
        public override BuildingType BuildingType => BuildingType.Mine;

        public IReadOnlyReactiveProperty<bool> CanUseMine => Game.Instance.TechController.CanUseMine;
        public IReadOnlyReactiveProperty<int> WorkerCount => Game.Instance.FolkManager.MineFolk;
        public string WorkersTitle => _minersTitle;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.FolkManager.MineFolk
                .TakeUntilDisable(this)
                .Subscribe(OnMinersCountChanged);
        }

        public void AddWorker()
        {
            Game.Instance.FolkManager.AddFolkToMine();
        }

        public void RemoveWorker()
        {
            Game.Instance.FolkManager.RemoveFolkFromMine();
        }

        public bool CanAddWorker()
        {
            return Game.Instance.FolkManager.FarmFolk.Value > 0 && Game.Instance.TechController.CanUseMine.Value;
        }

        public bool CanRemoveWorker()
        {
            return Game.Instance.FolkManager.MineFolk.Value > 0 && Game.Instance.TechController.CanUseMine.Value;
        }

        private void OnMinersCountChanged(int count)
        {
            _minersCount.text = count.ToString();
        }
    }
}