#nullable enable
using UnityEngine;

namespace NovemberProject.Treasures
{
    [CreateAssetMenu(menuName = "Create TreasureControllerSettings", fileName = "TreasureControllerSettings", order = 0)]
    public sealed class TreasureControllerSettings : ScriptableObject
    {
        [SerializeField]
        private int _startingTreasures = 0;

        public int StartingTreasures => _startingTreasures;
    }
}