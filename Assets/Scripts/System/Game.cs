#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.CameraSystem;
using NovemberProject.ClicheSpeech;
using NovemberProject.CoreGameplay;
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

namespace NovemberProject.System
{
    public sealed class Game : MonoBehaviour
    {
        private const string CLICHE_BIBLE_FILE = "cliche_bible";

        private readonly Subject<Unit> _onInitialized = new();

        private static Game _instance = null!;

        public static Game Instance => GetInstance();

        public ClicheBible ClicheBible { get; private set; } = null!;
        public RoundSystem RoundSystem { get; private set; } = null!;
        public TimeSystem TimeSystem { get; private set; } = null!;
        public InputSystem.InputSystem InputSystem { get; private set; } = null!;
        public UIManager UIManager { get; private set; } = null!;
        public GameStateMachine GameStateMachine { get; private set; } = null!;
        public CameraController CameraController { get; private set; } = null!;
        public BuildingSelector BuildingSelector { get; private set; } = null!;
        public MoneyController MoneyController { get; private set; } = null!;
        public FoodController FoodController { get; private set; } = null!;
        public StoneController StoneController { get; private set; } = null!;
        public TreasureController TreasureController { get; private set; } = null!;
        public ResourceMoveEffectSpawner ResourceMoveEffectSpawner { get; private set; } = null!;
        public BuildingsController BuildingsController { get; private set; } = null!;
        public TechController TechController { get; private set; } = null!;
        public Expeditions Expeditions { get; private set; } = null!;
        public CoreGameplay.CoreGameplay CoreGameplay { get; private set; } = null!;
        public IdleFolkSpawner IdleFolkSpawner { get; private set; } = null!;
        public FolkManager FolkManager => CoreGameplay.FolkManager;
        public ArmyManager ArmyManager => CoreGameplay.ArmyManager;
        public bool IsInitialized { get; private set; }

        public IObservable<Unit> OnInitialized => _onInitialized;
        public IObservable<State> OnStateChanged => GameStateMachine.OnStateChanged;
        public MessageBroker MessageBroker { get; } = new();

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
            MoneyController = FindObjectOfType<MoneyController>();
            ResourceMoveEffectSpawner = FindObjectOfType<ResourceMoveEffectSpawner>();
            BuildingsController = FindObjectOfType<BuildingsController>();
            CoreGameplay = FindObjectOfType<CoreGameplay.CoreGameplay>();
            FoodController = FindObjectOfType<FoodController>();
            StoneController = FindObjectOfType<StoneController>();
            TreasureController = FindObjectOfType<TreasureController>();
            TechController = FindObjectOfType<TechController>();
            Expeditions = FindObjectOfType<Expeditions>();
            IdleFolkSpawner = FindObjectOfType<IdleFolkSpawner>();
        }

        private void Initialize()
        {
            ClicheBible = new ClicheBible(CLICHE_BIBLE_FILE);
            GameStateMachine = new GameStateMachine();
            _onInitialized.OnNext(Unit.Default);
            IsInitialized = true;
        }
    }
}