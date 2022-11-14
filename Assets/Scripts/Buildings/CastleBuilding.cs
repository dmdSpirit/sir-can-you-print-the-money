#nullable enable
using NovemberProject.InfoPanels.UI;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class CastleBuilding : Building
    {
        [SerializeField]
        private InfoPanel _infoPanel = null!;
    }
}