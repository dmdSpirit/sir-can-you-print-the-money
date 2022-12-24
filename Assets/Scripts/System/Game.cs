#nullable enable
using System;
using NovemberProject.GameStates;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.System
{
    public sealed class Game : MonoBehaviour
    {
        private readonly Subject<Unit> _onInitialized = new();

        private static Game _instance = null!;

        private GameStateMachine _gameStateMachine = null!;

        public static Game Instance => GetInstance();

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

        private void Initialize()
        {
            _onInitialized.OnNext(Unit.Default);
            IsInitialized = true;
        }
    }
}