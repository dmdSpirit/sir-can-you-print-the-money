#nullable enable
using UnityEngine;

namespace NovemberProject.Time
{
    [CreateAssetMenu(menuName = "Create TimeSystemSettings", fileName = "TimeSystemSettings", order = 0)]
    public sealed class TimeSystemSettings : ScriptableObject
    {
        [SerializeField]
        private float _speedUpScale = 8f;

        public float SpeedUpScale => _speedUpScale;
    }
}