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