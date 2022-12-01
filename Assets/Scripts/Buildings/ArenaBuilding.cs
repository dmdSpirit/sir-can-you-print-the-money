#nullable enable
using NovemberProject.System;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NovemberProject.Buildings
{
    public sealed class ArenaBuilding : Building, IConstructableBuilding
    {
        private readonly ReactiveProperty<ConstructableState> _constructableState = new();
        private Timer? _constructionTimer;

        [SerializeField]
        private int _constructionCost;

        [SerializeField]
        private Sprite _stoneResourceImage = null!;

        [SerializeField]
        private float _constructionDuration;

        [SerializeField]
        private TMP_Text _panelText = null!;

        [SerializeField]
        private Image _panelImage = null!;

        [SerializeField]
        private GameObject _panel = null!;

        public override BuildingType BuildingType => BuildingType.Arena;

        public IReadOnlyReactiveProperty<ConstructableState> ConstructableState => _constructableState;
        public int ConstructionCost => _constructionCost;
        public Sprite ResourceImage => _stoneResourceImage;
        public IReadOnlyTimer? ConstructionTimer => _constructionTimer;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.MessageBroker.Receive<NewGameMessage>()
                .TakeUntilDisable(this)
                .Subscribe(OnNewGame);
            Game.Instance.StoneController.Stone
                .TakeUntilDisable(this)
                .Subscribe(OnStoneCountChanged);
        }

        public void StartConstruction()
        {
            Assert.IsTrue(Game.Instance.StoneController.Stone.Value >= _constructionCost);
            Assert.IsTrue(ConstructableState.Value == Buildings.ConstructableState.NotConstructed);
            _constructionTimer = Game.Instance.TimeSystem.CreateTimer(_constructionDuration, OnConstructionFinished);
            Game.Instance.StoneController.SpendStone(_constructionCost);
            _constructionTimer.Start();
            _constructableState.Value = Buildings.ConstructableState.IsConstructing;
            _panel.SetActive(false);
        }

        private void OnNewGame(NewGameMessage _)
        {
            _constructableState.Value = Buildings.ConstructableState.NotConstructed;
            _panel.SetActive(true);
            _panelImage.sprite = _stoneResourceImage;
        }

        private void OnConstructionFinished(Timer _)
        {
            Assert.IsTrue(_constructableState.Value == Buildings.ConstructableState.IsConstructing);
            _constructableState.Value = Buildings.ConstructableState.Constructed;
            _constructionTimer = null;
            _panel.SetActive(false);
            Game.Instance.GameStateMachine.Victory();
        }

        private void OnStoneCountChanged(int stone)
        {
            if (_constructableState.Value != Buildings.ConstructableState.NotConstructed)
            {
                return;
            }

            _panelText.text = $"{stone}/{_constructionCost}";
        }
    }
}