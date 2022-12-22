#nullable enable
using UnityEngine;

namespace NovemberProject.Buildings
{
    [CreateAssetMenu(menuName = "Create BuildingSelectorSettings", fileName = "BuildingSelectorSettings", order = 0)]
    public class BuildingSelectorSettings : ScriptableObject
    {
        [SerializeField]
        private LayerMask _buildingLayerMask;

        public LayerMask BuildingLayerMask => _buildingLayerMask;
    }
}