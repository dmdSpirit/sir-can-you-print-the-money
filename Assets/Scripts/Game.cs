#nullable enable
using UnityEngine;

namespace NovemberProject
{
    public class Game : MonoBehaviour
    {
        private const string CLICHE_BIBLE_FILE = "cliche_bible";

        private static Game _instance = null!;

        public ClicheBible ClicheBible { get; private set; } = null!;

        public static Game Instance
        {
            get
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

                var instanceObject = new GameObject(nameof(Game));
                _instance = instanceObject.AddComponent<Game>();
                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Initialize();
        }

        private void Initialize()
        {
            ClicheBible = new ClicheBible(CLICHE_BIBLE_FILE);
        }
    }
}