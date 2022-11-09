#nullable enable
using System;
using NovemberProject.CameraSystem;
using NovemberProject.ClicheSpeech;
using NovemberProject.GameStates;
using NovemberProject.Rounds;
using NovemberProject.System.UI;
using NovemberProject.Time;
using UniRx;
using UnityEngine;

namespace NovemberProject.System
{
    public class Game : MonoBehaviour
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
        public bool IsInitialized { get; private set; }

        public IObservable<Unit> OnInitialized => _onInitialized;
        public IObservable<State> OnStateChanged => GameStateMachine.OnStateChanged;

        private void Start()
        {
            DontDestroyOnLoad(this);
            CreateComponents();
            Initialize();
            GameStateMachine.InitializeGame();
        }

        public void NewGame() => GameStateMachine.NewGame();
        public void ExitGame() => GameStateMachine.ExitGame();
        public void MainMenu() => GameStateMachine.MainMenu();
        public void FinishRound() => GameStateMachine.FinishRound();
        public void NextRound() => GameStateMachine.Turn();

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