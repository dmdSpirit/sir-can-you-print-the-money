#nullable enable
using UnityEngine;

namespace NovemberProject.CoreGameplay
{
    [CreateAssetMenu(menuName = "Configs/RoundStartConfigs", fileName = "RoundStartConfigs", order = 0)]
    public sealed class RoundStartConfigs : ScriptableObject
    {
        [SerializeField]
        private RoundStartConfig[] _roundStartConfigs=null!;

        [SerializeField]
        private RoundStartConfig _defaultStartConfig = null!;

        public RoundStartConfig GetRoundStartConfig(int roundNumber)
        {
            if (roundNumber > _roundStartConfigs.Length)
            {
                return _defaultStartConfig;
            }

            return _roundStartConfigs[roundNumber - 1];
        }
    }
}