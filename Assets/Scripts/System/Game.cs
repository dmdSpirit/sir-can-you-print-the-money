#nullable enable
using System;
using NovemberProject.ClicheSpeech;
using NovemberProject.CoreGameplay;
using NovemberProject.GameStates;
using NovemberProject.MovingResources;
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
        public ResourceMoveEffectSpawner ResourceMoveEffectSpawner { get; private set; } = null!;
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
            ResourceMoveEffectSpawner = FindObjectOfType<ResourceMoveEffectSpawner>();
            CoreGameplay = FindObjectOfType<CoreGameplay.CoreGameplay>();
        }

        private void Initialize()
        {
            ClicheBible = new ClicheBible(CLICHE_BIBLE_FILE);
            _onInitialized.OnNext(Unit.Default);
            IsInitialized = true;
        }
    }
}