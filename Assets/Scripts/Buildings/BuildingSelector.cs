#nullable enable
using NovemberProject.Buildings.UI;
using NovemberProject.System.UI;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public class BuildingSelector
    {
        private readonly BuildingSelectorSettings _settings;
        private readonly UIManager _uiManager;

        private Building? _selectedBuilding;
        private bool _isSelected;

        public LayerMask LayerMask => _settings.BuildingLayerMask;
        
        public BuildingSelector(BuildingSelectorSettings buildingSelectorSettings, UIManager uiManager)
        {
            _settings = buildingSelectorSettings;
            _uiManager = uiManager;
        }

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

            var buildingInfoPanel = _uiManager.GetScreen<IBuildingInfoPanel>();
            buildingInfoPanel.SetBuilding(building);
            buildingInfoPanel.Show();
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
            var buildingInfoPanel = _uiManager.GetScreen<IBuildingInfoPanel>();
            buildingInfoPanel.Hide();
        }
    }
}