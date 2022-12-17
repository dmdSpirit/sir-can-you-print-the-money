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

        public override void HandleInput()
        {
            if (Game.Instance.UIManager.IsMouseOver.Value)
            {
                if (Game.Instance.BuildingNameHover.IsShowing)
                {
                    Game.Instance.BuildingNameHover.HidePanel();
                }

                return;
            }

            CameraController cameraController = Game.Instance.CameraController;
            BuildingSelector buildingSelector = Game.Instance.BuildingSelector;
            RaycastBuildingSelection(cameraController, buildingSelector);
        }

        private static void RaycastBuildingSelection(CameraController cameraController,
            BuildingSelector buildingSelector)
        {
            Ray ray = cameraController.MainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            UIManager uiManager = Game.Instance.UIManager;
            LayerMask layerMask = buildingSelector.LayerMask | uiManager.LayerMask;
            if (Physics.Raycast(ray, out RaycastHit hit, RAYCAST_MAX_DISTANCE, layerMask: layerMask))
            {
                GameObject hitObject = hit.transform.gameObject;
                var building = hitObject.GetComponent<Building>();
                if (building != null)
                {
                    if (UnityEngine.Input.GetMouseButton(LEFT_MOUSE_BUTTON))
                    {
                        Game.Instance.BuildingSelector.Select(building);
                    }

                    Game.Instance.BuildingNameHover.ShowName(building);
                }

                return;
            }

            Game.Instance.BuildingNameHover.HidePanel();
            if (UnityEngine.Input.GetMouseButton(LEFT_MOUSE_BUTTON))
            {
                Game.Instance.BuildingSelector.Unselect();
            }
        }
    }
}