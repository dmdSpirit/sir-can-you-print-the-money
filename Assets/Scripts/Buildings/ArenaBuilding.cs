#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.GameStates;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class ArenaBuilding : Building, IConstructableBuilding
    {
        private readonly ReactiveProperty<ConstructableState> _constructableState = new();

        private MessageBroker _messageBroker = null!;
        private GameStateMachine _gameStateMachine = null!;
        private TimeSystem _timeSystem = null!;
        private StoneController _stoneController = null!;
        private Timer? _constructionTimer;
        private Vector3 _initialPosition;

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

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, TimeSystem timeSystem,
            StoneController stoneController, MessageBroker messageBroker)
        {
            _gameStateMachine = gameStateMachine;
            _timeSystem = timeSystem;
            _stoneController = stoneController;
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
            _stoneController.Stone.Subscribe(OnStoneCountChanged);
            _initialPosition = transform.position;
        }

        public void StartConstruction()
        {
            Assert.IsTrue(_stoneController.Stone.Value >= _constructionCost);
            Assert.IsTrue(ConstructableState.Value == Buildings.ConstructableState.NotConstructed);
            _constructionTimer = _timeSystem.CreateTimer(_constructionDuration, OnConstructionFinished);
            _stoneController.SpendStone(_constructionCost);
            _constructionTimer.Start();
            _constructableState.Value = Buildings.ConstructableState.IsConstructing;
            _panel.SetActive(false);
        }

        private void OnNewGame(NewGameMessage _)
        {
            _constructableState.Value = Buildings.ConstructableState.NotConstructed;
            _panel.SetActive(true);
            _panelImage.sprite = _stoneResourceImage;
            transform.position = _initialPosition;
        }

        private void OnConstructionFinished(Timer _)
        {
            Assert.IsTrue(_constructableState.Value == Buildings.ConstructableState.IsConstructing);
            _constructableState.Value = Buildings.ConstructableState.Constructed;
            _constructionTimer = null;
            _panel.SetActive(false);
            _gameStateMachine.Victory();
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