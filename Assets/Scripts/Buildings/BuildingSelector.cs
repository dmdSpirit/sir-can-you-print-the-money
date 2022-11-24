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

            if (_selectedBuilding is ISelectable previousSelectable)
            {
                previousSelectable.Unselect();
            }
            _selectedBuilding = building;
            _isSelected = true;
            if (building is ISelectable selectable)
            {
                selectable.Select();
            }

            Game.Instance.UIManager.ShowBuildingInfo(building);
        }

        public void Unselect()
        {
            if (!_isSelected)
            {
                return;
            }

            if (_selectedBuilding is ISelectable selectable)
            {
                selectable.Unselect();
            }

            _selectedBuilding = null;
            _isSelected = false;
            Game.Instance.UIManager.HideBuildingInfo();
        }
    }
}