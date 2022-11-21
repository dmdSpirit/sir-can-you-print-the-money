#nullable enable
using NovemberProject.Buildings;
using NovemberProject.CameraSystem;
using NovemberProject.System;
using NovemberProject.System.UI;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public sealed class MouseSelectionHandler : InputHandler
    {
        private const int LEFT_MOUSE_BUTTON = 0;
        private const float RAYCAST_MAX_DISTANCE = 1000f;

        public override void HandleInput()
        {
            if (!Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON) || Game.Instance.UIManager.IsMouseOver.Value)
            {
                return;
            }

            CameraController cameraController = Game.Instance.CameraController;
            BuildingSelector buildingSelector = Game.Instance.BuildingSelector;
            RaycastBuildingSelection(cameraController, buildingSelector);
        }

        private static void RaycastBuildingSelection(CameraController cameraController, BuildingSelector buildingSelector)
        {
            Ray ray = cameraController.MainCamera.ScreenPointToRay(Input.mousePosition);
            UIManager uiManager = Game.Instance.UIManager;
            LayerMask layerMask = buildingSelector.LayerMask | uiManager.LayerMask;
            if (Physics.Raycast(ray, out RaycastHit hit, RAYCAST_MAX_DISTANCE, layerMask: layerMask))
            {
                GameObject hitObject = hit.transform.gameObject;
                var building = hitObject.GetComponent<Building>();
                if (building != null)
                {
                    Game.Instance.BuildingSelector.Select(building);
                }

                return;
            }

            Game.Instance.BuildingSelector.Unselect();
        }
    }
}