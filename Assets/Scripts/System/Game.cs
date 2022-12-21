#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.ClicheSpeech;
using NovemberProject.CoreGameplay;
using NovemberProject.GameStates;
using NovemberProject.MovingResources;
using NovemberProject.System.UI;
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

        private GameStateMachine _gameStateMachine = null!;

        public static Game Instance => GetInstance();

        public ClicheBible ClicheBible { get; private set; } = null!;
        public UIManager UIManager { get; private set; } = null!;
        public ResourceMoveEffectSpawner ResourceMoveEffectSpawner { get; private set; } = null!;
        public CombatController CombatController { get; private set; } = null!;
        public CoreGameplay.CoreGameplay CoreGameplay { get; private set; } = null!;
        public bool IsInitialized { get; private set; }

        public IObservable<Unit> OnInitialized => _onInitialized;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            CreateComponents();
            Initialize();
            _gameStateMachine.InitializeGame();
        }

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
            UIManager = FindObjectOfType<UIManager>();
            ResourceMoveEffectSpawner = FindObjectOfType<ResourceMoveEffectSpawner>();
            CoreGameplay = FindObjectOfType<CoreGameplay.CoreGameplay>();
            CombatController = FindObjectOfType<CombatController>();
        }

        private void Initialize()
        {
            ClicheBible = new ClicheBible(CLICHE_BIBLE_FILE);
            _onInitialized.OnNext(Unit.Default);
            IsInitialized = true;
        }
    }
}