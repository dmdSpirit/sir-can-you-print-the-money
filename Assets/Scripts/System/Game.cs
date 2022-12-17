#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.CameraSystem;
using NovemberProject.ClicheSpeech;
using NovemberProject.CoreGameplay;
using NovemberProject.CoreGameplay.FolkManagement;
using NovemberProject.GameStates;
using NovemberProject.MovingResources;
using NovemberProject.Rounds;
using NovemberProject.System.Messages;
using NovemberProject.System.UI;
using NovemberProject.TechTree;
using NovemberProject.Time;
using NovemberProject.Treasures;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.System
{
    public sealed class Game : MonoBehaviour
    {
        private const string CLICHE_BIBLE_FILE = "cliche_bible";

        private readonly Subject<Unit> _onInitialized = new();

        private static Game _instance = null!;

        private MessageBroker _messageBroker = null!;
        private GameStateMachine _gameStateMachine = null!;

        public static Game Instance => GetInstance();

        public ClicheBible ClicheBible { get; private set; } = null!;
        public RoundSystem RoundSystem { get; private set; } = null!;
        public TimeSystem TimeSystem { get; private set; } = null!;
        public InputSystem.InputSystem InputSystem { get; private set; } = null!;
        public UIManager UIManager { get; private set; } = null!;
        public GameStateMachine GameStateMachine { get; private set; } = null!;
        public CameraController CameraController { get; private set; } = null!;
        public BuildingSelector BuildingSelector { get; private set; } = null!;
        public StoneController StoneController { get; private set; } = null!;
        public TreasureController TreasureController { get; private set; } = null!;
        public ResourceMoveEffectSpawner ResourceMoveEffectSpawner { get; private set; } = null!;
        public TechController TechController { get; private set; } = null!;
        public CombatController CombatController { get; private set; } = null!;
        public CoreGameplay.CoreGameplay CoreGameplay { get; private set; } = null!;
        public AudioManager AudioManager { get; private set; } = null!;
        public BuildingNameHover BuildingNameHover { get; private set; } = null!;
        public bool IsInitialized { get; private set; }

        public IObservable<Unit> OnInitialized => _onInitialized;
        public IObservable<State> OnStateChanged => GameStateMachine.OnStateChanged;
        public MessageBroker MessageBroker =>_messageBroker;

        [Inject]
        private void Construct(MessageBroker messageBroker, GameStateMachine gameStateMachine)
        {
            _messageBroker = messageBroker;
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            CreateComponents();
            Initialize();
            GameStateMachine.InitializeGame();
        }

        public static void PublishMessage(IMessage message) => Instance.MessageBroker.Publish(message);

        private static Game GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = FindObjectOfType<Game>();
            if (_instance != null)
            {
                return _instance;
            }

            var obj = new GameObject(nameof(Game));
            _instance = obj.AddComponent<Game>();
            return _instance;
        }

        private void CreateComponents()
        {
            RoundSystem = GetComponent<RoundSystem>();
            TimeSystem = gameObject.AddComponent<TimeSystem>();
            InputSystem = gameObject.AddComponent<InputSystem.InputSystem>();
            UIManager = FindObjectOfType<UIManager>();
            CameraController = FindObjectOfType<CameraController>();
            BuildingSelector = FindObjectOfType<BuildingSelector>();
            ResourceMoveEffectSpawner = FindObjectOfType<ResourceMoveEffectSpawner>();
            CoreGameplay = FindObjectOfType<CoreGameplay.CoreGameplay>();
            StoneController = FindObjectOfType<StoneController>();
            TreasureController = FindObjectOfType<TreasureController>();
            TechController = FindObjectOfType<TechController>();
            CombatController = FindObjectOfType<CombatController>();
            AudioManager = FindObjectOfType<AudioManager>();
            BuildingNameHover = FindObjectOfType<BuildingNameHover>();
        }

        private void Initialize()
        {
            ClicheBible = new ClicheBible(CLICHE_BIBLE_FILE);
            _onInitialized.OnNext(Unit.Default);
            IsInitialized = true;
        }
    }
}