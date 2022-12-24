#nullable enable
using System;
using System.Collections.Generic;
using NovemberProject.Buildings;
using NovemberProject.CameraSystem;
using NovemberProject.System.UI;
using NovemberProject.Time;

namespace NovemberProject.Input
{
    public sealed class InputHandlersFactory
    {
        private readonly Dictionary<Type, InputHandler> _inputHandlers = new();

        public T GetInputHandler<T>() where T : InputHandler, new()
        {
            if (!_inputHandlers.ContainsKey(typeof(T)))
            {
                _inputHandlers.Add(typeof(T), new T());
            }

            return (T)_inputHandlers[typeof(T)];
        }

        public TimeControlsHandler GetTimeControlsHandler(TimeSystem timeSystem)
        {
            if (!_inputHandlers.ContainsKey(typeof(TimeControlsHandler)))
            {
                _inputHandlers.Add(typeof(TimeControlsHandler), new TimeControlsHandler(timeSystem));
            }

            return (TimeControlsHandler)_inputHandlers[typeof(TimeControlsHandler)];
        }

        public MouseSelectionHandler GetMouseSelectionHandler(CameraController cameraController,
            BuildingNameHover buildingNameHover, BuildingSelector buildingSelector, UIManager uiManager)
        {
            if (!_inputHandlers.ContainsKey(typeof(MouseSelectionHandler)))
            {
                _inputHandlers.Add(typeof(MouseSelectionHandler),
                    new MouseSelectionHandler(cameraController, buildingNameHover, buildingSelector, uiManager));
            }

            return (MouseSelectionHandler)_inputHandlers[typeof(MouseSelectionHandler)];
        }

        public MoveCameraHandler GetMoveCameraHandler(CameraController cameraController, UIManager uiManager)
        {
            if (!_inputHandlers.ContainsKey(typeof(MoveCameraHandler)))
            {
                _inputHandlers.Add(typeof(MoveCameraHandler), new MoveCameraHandler(cameraController, uiManager));
            }

            return (MoveCameraHandler)_inputHandlers[typeof(MoveCameraHandler)];
        }

        public ToggleCheatMenuInputHandler GetToggleCheatMenuInputHandler(UIManager uiManager)
        {
            if (!_inputHandlers.ContainsKey(typeof(ToggleCheatMenuInputHandler)))
            {
                _inputHandlers.Add(typeof(ToggleCheatMenuInputHandler), new ToggleCheatMenuInputHandler(uiManager));
            }

            return (ToggleCheatMenuInputHandler)_inputHandlers[typeof(ToggleCheatMenuInputHandler)];
        }

        public ToggleSystemPanelInputHandler GetToggleSystemPanelInputHandler(UIManager uiManager)
        {
            if (!_inputHandlers.ContainsKey(typeof(ToggleSystemPanelInputHandler)))
            {
                _inputHandlers.Add(typeof(ToggleSystemPanelInputHandler), new ToggleSystemPanelInputHandler(uiManager));
            }

            return (ToggleSystemPanelInputHandler)_inputHandlers[typeof(ToggleSystemPanelInputHandler)];
        }
    }
}