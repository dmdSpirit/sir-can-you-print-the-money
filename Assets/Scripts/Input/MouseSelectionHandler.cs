#nullable enable
using NovemberProject.Buildings;
using NovemberProject.CameraSystem;
using NovemberProject.System;
using NovemberProject.System.UI;
using UnityEngine;

namespace NovemberProject.Input
{
    public sealed class MouseSelectionHandler : InputHandler
    {
        private const int LEFT_MOUSE_BUTTON = 0;
        private const float RAYCAST_MAX_DISTANCE = 1000f;

        private readonly CameraController _cameraController;
        private readonly BuildingNameHover _buildingNameHover;
        private readonly BuildingSelector _buildingSelector;
        private readonly UIManager _uiManager;

        public MouseSelectionHandler(CameraController cameraController, BuildingNameHover buildingNameHover,
            BuildingSelector buildingSelector, UIManager uiManager)
        {
            _cameraController = cameraController;
            _buildingNameHover = buildingNameHover;
            _buildingSelector = buildingSelector;
            _uiManager = uiManager;
        }

        public override void HandleInput()
        {
            if (_uiManager.IsMouseOver.Value)
            {
                if (_buildingNameHover.IsShowing)
                {
                    _buildingNameHover.HidePanel();
                }

                return;
            }

            RaycastBuildingSelection();
        }

        private void RaycastBuildingSelection()
        {
            Ray ray = _cameraController.MainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            LayerMask layerMask = _buildingSelector.LayerMask | _uiManager.LayerMask;
            if (Physics.Raycast(ray, out RaycastHit hit, RAYCAST_MAX_DISTANCE, layerMask: layerMask))
            {
                GameObject hitObject = hit.transform.gameObject;
                var building = hitObject.GetComponent<Building>();
                if (building != null)
                {
                    if (UnityEngine.Input.GetMouseButton(LEFT_MOUSE_BUTTON))
                    {
                        _buildingSelector.Select(building);
                    }

                    _buildingNameHover.ShowName(building);
                }

                return;
            }

            _buildingNameHover.HidePanel();
            if (UnityEngine.Input.GetMouseButton(LEFT_MOUSE_BUTTON))
            {
                _buildingSelector.Unselect();
            }
        }
    }
}