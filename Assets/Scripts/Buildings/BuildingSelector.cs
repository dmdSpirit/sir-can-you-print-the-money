#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public class BuildingSelector : InitializableBehaviour
    {
        private Building? _selectedBuilding;
        private bool _isSelected;

        [SerializeField]
        private LayerMask _layerMask;

        public LayerMask LayerMask => _layerMask;

        public void Select(Building building)
        {
            if (_isSelected && _selectedBuilding == building)
            {
                return;
            }

            _selectedBuilding = building;
            _isSelected = true;
            Game.Instance.UIManager.ShowBuildingInfo(building);
        }

        public void Unselect()
        {
            if (!_isSelected)
            {
                return;
            }

            _selectedBuilding = null;
            _isSelected = false;
            Game.Instance.UIManager.HideBuildingInfo();
        }
    }
}