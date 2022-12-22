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

        public MouseSelectionHandler(CameraController cameraController, BuildingNameHover buildingNameHover,
            BuildingSelector buildingSelector)
        {
            _cameraController = cameraController;
            _buildingNameHover = buildingNameHover;
            _buildingSelector = buildingSelector;
        }

        public override void HandleInput()
        {
            if (Game.Instance.UIManager.IsMouseOver.Value)
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
            UIManager uiManager = Game.Instance.UIManager;
            LayerMask layerMask = _buildingSelector.LayerMask | uiManager.LayerMask;
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