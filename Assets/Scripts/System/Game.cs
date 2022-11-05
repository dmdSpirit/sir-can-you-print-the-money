#nullable enable
using System;
using NovemberProject.ClicheSpeech;
using NovemberProject.Input;
using NovemberProject.RoundS;
using UniRx;
using UnityEngine;

namespace NovemberProject.System
{
    [RequireComponent(typeof(RoundSystem))]
    [RequireComponent(typeof(TimeSystem.TimeSystem))]
    [RequireComponent(typeof(InputSystem))]
    public class Game : MonoBehaviour
    {
        private const string CLICHE_BIBLE_FILE = "cliche_bible";

        private readonly Subject<Unit> _onInitialized = new();

        private static Game _instance = null!;

        public static Game Instance => GetInstance();

        public ClicheBible ClicheBible { get; private set; } = null!;
        public RoundSystem RoundSystem { get; private set; } = null!;
        public TimeSystem.TimeSystem TimeSystem { get; private set; } = null!;
        public InputSystem InputSystem { get; private set; } = null!;

        public IObservable<Unit> OnInitialized => _onInitialized;

        private void Start()
        {
            DontDestroyOnLoad(this);
            GetComponents();
            Initialize();
        }

        private static Game CreateGameObject()
        {
            var instanceObject = new GameObject(nameof(Game));
            var game = instanceObject.AddComponent<Game>();
            return game;
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

            _instance = CreateGameObject();
            return _instance;
        }

        private void GetComponents()
        {
            RoundSystem = GetComponent<RoundSystem>();
            TimeSystem = GetComponent<TimeSystem.TimeSystem>();
            InputSystem = GetComponent<InputSystem>();
        }

        private void Initialize()
        {
            ClicheBible = new ClicheBible(CLICHE_BIBLE_FILE);
            TimeSystem.Initialize();
            InputSystem.Initialize();
            _onInitialized.OnNext(Unit.Default);
        }
    }
}