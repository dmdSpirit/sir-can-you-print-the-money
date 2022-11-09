#nullable enable
using NovemberProject.CameraSystem;
using NovemberProject.System;
using NovemberProject.System.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.InputSystem
{
    public class MouseSelectionHandler : InputHandler
    {
        private const int LEFT_MOUSE_BUTTON = 0;
        private const float RAYCAST_MAX_DISTANCE = 1000f;

        public override void HandleInput()
        {
            if (!Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
            {
                return;
            }

            CameraController cameraController = Game.Instance.CameraController;
            Camera camera = cameraController.MainCamera;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            BuildingSelector buildingSelector = Game.Instance.BuildingSelector;
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
            }
            else
            {
                Game.Instance.BuildingSelector.Unselect();
            }
        }
    }
}