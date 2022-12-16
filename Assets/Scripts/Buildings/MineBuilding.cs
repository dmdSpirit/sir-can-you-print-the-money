#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.CoreGameplay.FolkManagement;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class MineBuilding : Building, IMineWorkerManipulator, IProducer
    {
        private readonly ReactiveProperty<int> _producedValue = new();
        private readonly ReactiveProperty<bool> _isProducing = new();

        private Timer? _productionTimer;
        private int _minersProducing;
        private FolkManager _folkManager = null!;

        [SerializeField]
        private string _minersTitle = "Miners";

        [SerializeField]
        private TMP_Text _minersCount = null!;

        [SerializeField]
        private int _productionPerMiner = 1;

        [SerializeField]
        private float _productionTime = 6;

        public override BuildingType BuildingType => BuildingType.Mine;

        public IReadOnlyReactiveProperty<bool> CanUseMine => Game.Instance.TechController.CanUseMine;
        public IReadOnlyReactiveProperty<int> WorkerCount => _folkManager.MineFolk;
        public string WorkersTitle => _minersTitle;
        public bool ShowProducedValue => true;
        public IReadOnlyReactiveProperty<int> ProducedValue => _producedValue;
        public IReadOnlyReactiveProperty<bool> IsProducing => _isProducing;
        public IReadOnlyTimer? ProductionTimer => _productionTimer;

        [Inject]
        private void Construct(FolkManager folkManager)
        {
            _folkManager = folkManager;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _folkManager.MineFolk
                .TakeUntilDisable(this)
                .Subscribe(OnMinersCountChanged);
        }

        public void AddWorker() => _folkManager.AddFolkToMine();
        public void RemoveWorker() => _folkManager.RemoveFolkFromMine();
        public bool CanAddWorker() => _folkManager.CanAddWorkerToMine();

        public bool CanRemoveWorker() => _folkManager.CanRemoveMineWorker();

        private void OnMinersCountChanged(int miners)
        {
            _minersCount.text = miners.ToString();
            if (miners == 0)
            {
                StopProduction();
                return;
            }

            if (!_isProducing.Value)
            {
                StartProduction();
                return;
            }

            if (miners < _minersProducing)
            {
                _minersProducing = miners;
                _producedValue.Value = _minersProducing * _productionPerMiner;
            }
        }


        private void StartProduction()
        {
            int miners = _folkManager.MineFolk.Value;
            Assert.IsTrue(miners > 0);
            Assert.IsTrue(_productionTimer == null);
            _productionTimer = Game.Instance.TimeSystem.CreateTimer(_productionTime, OnStoneProduced);
            _minersProducing = miners;
            _producedValue.Value = _minersProducing * _productionPerMiner;
            _productionTimer.Start();
            _isProducing.Value = true;
        }

        private void StopProduction()
        {
            if (!_isProducing.Value)
            {
                return;
            }

            _productionTimer?.Cancel();
            _productionTimer = null;
            _producedValue.Value = 0;
            _isProducing.Value = false;
        }

        private void OnStoneProduced(Timer _)
        {
            Assert.IsTrue(_productionTimer != null);
            Assert.IsTrue(_isProducing.Value);
            Assert.IsTrue(_folkManager.MineFolk.Value > 0);
            Assert.IsTrue(_minersProducing > 0);
            Game.Instance.StoneController.MineStone(_productionPerMiner * _minersProducing);
            _productionTimer = null;
            _isProducing.Value = false;
            StartProduction();
        }
    }
}