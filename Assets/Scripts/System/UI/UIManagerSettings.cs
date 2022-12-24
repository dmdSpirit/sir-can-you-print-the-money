#nullable enable
using UnityEngine;

namespace NovemberProject.System.UI
{
    [CreateAssetMenu(menuName = "Create UIManagerSettings", fileName = "UIManagerSettings", order = 0)]
    public sealed class UIManagerSettings : ScriptableObject
    {
        [SerializeField]
        private LayerMask _layerMask;

        public LayerMask LayerMask => _layerMask;
    }
}