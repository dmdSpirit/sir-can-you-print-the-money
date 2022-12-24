#nullable enable
using UnityEngine;

namespace NovemberProject.Core
{
    [CreateAssetMenu(menuName = "Create CoreGameplaySettings", fileName = "CoreGameplaySettings", order = 0)]
    public sealed class CoreGameplaySettings : ScriptableObject
    {
        [SerializeField]
        private float _folkEatTime = 40f;

        [SerializeField]
        private float _folkPayTime = 30f;

        [SerializeField]
        private float _armyEatTime = 20f;

        [SerializeField]
        private float _armyPayTime = 10f;

        public float FolkEatTime => _folkEatTime;
        public float FolkPayTime => _folkPayTime;
        public float ArmyEatTime => _armyEatTime;
        public float ArmyPayTime => _armyPayTime;
    }
}