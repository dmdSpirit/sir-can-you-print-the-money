#nullable enable
using UnityEngine;

namespace NovemberProject.Core
{
    [CreateAssetMenu(menuName = "Create StoneControllerSettings", fileName = "StoneControllerSettings", order = 0)]
    public sealed class StoneControllerSettings : ScriptableObject
    {
        [SerializeField]
        private int _startingStone = 0;

        public int StatingStone => _startingStone;
    }
}