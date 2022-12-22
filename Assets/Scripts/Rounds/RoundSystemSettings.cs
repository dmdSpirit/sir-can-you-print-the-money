#nullable enable
using UnityEngine;

namespace NovemberProject.Rounds
{
    [CreateAssetMenu(menuName = "Create RoundSystemSettings", fileName = "RoundSystemSettings", order = 0)]
    public sealed class RoundSystemSettings : ScriptableObject
    {
        [SerializeField]
        private float _roundDuration = 50f;

        public float RoundDuration => _roundDuration;
    }
}