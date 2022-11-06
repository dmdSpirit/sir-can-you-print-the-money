#nullable enable
using System;
using NovemberProject.ClicheSpeech;
using NovemberProject.Input;
using NovemberProject.RoundS;
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
        public TimeSystem.TimeSystem TimeSystem { get; private set; } = null!;
        public InputSystem InputSystem { get; private set; } = null!;
        public MessageBroker MessageBroker { get; private set; } = null!;
        public bool IsInitialized { get; private set; }

        public IObservable<Unit> OnInitialized => _onInitialized;

        private void Start()
        {
            DontDestroyOnLoad(this);
            CreateComponents();
            Initialize();
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
            RoundSystem = gameObject.AddComponent<RoundSystem>();
            TimeSystem = gameObject.AddComponent<TimeSystem.TimeSystem>();
            InputSystem = gameObject.AddComponent<InputSystem>();
        }

        private void Initialize()
        {
            ClicheBible = new ClicheBible(CLICHE_BIBLE_FILE);
            MessageBroker = new MessageBroker();
            _onInitialized.OnNext(Unit.Default);
            IsInitialized = true;
        }
    }
}