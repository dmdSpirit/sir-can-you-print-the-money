#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace NovemberProject.Core
{
    [CreateAssetMenu(menuName = "Create ExpeditionSettings", fileName = "ExpeditionSettings", order = 0)]
    public sealed class ExpeditionSettings : ScriptableObject
    {
        [SerializeField]
        private int _expeditionDuration = 25;

        [SerializeField]
        private ExpeditionData[] _expeditionDatas = null!;

        public int ExpeditionDuration => _expeditionDuration;
        public IReadOnlyList<ExpeditionData> ExpeditionDatas => _expeditionDatas;
    }
}