#nullable enable
using System;
using System.Collections.Generic;
using NovemberProject.Buildings;
using NovemberProject.CameraSystem;
using NovemberProject.CommonUIStuff;
using NovemberProject.System.UI;
using NovemberProject.Time;
using UniRx;
using UnityEngine;

namespace NovemberProject.Input
{
    public sealed class InputSystem : InitializableBehaviour
    {
        private readonly InputHandlersFactory _inputHandlersFactory = new();
        private readonly List<InputHandler> _globalInputHandlers = new();
        private readonly Subject<Unit> _onHandleInput = new();

        private bool _isActive;

        public IObservable<Unit> OnHandleInput => _onHandleInput;

        private void Start()
        {
            _isActive = true;
        }

        public T GetInputHandler<T>() where T : InputHandler, new() => _inputHandlersFactory.GetInputHandler<T>();

        public TimeControlsHandler GetTimeControlsHandler(TimeSystem timeSystem) =>
            _inputHandlersFactory.GetTimeControlsHandler(timeSystem);

        public MoveCameraHandler GetMoveCameraHandler(CameraController cameraController) =>
            _inputHandlersFactory.GetMoveCameraHandler(cameraController);

        public MouseSelectionHandler GetMouseSelectionHandler(CameraController cameraController,
            BuildingNameHover buildingNameHover, BuildingSelector buildingSelector) =>
            _inputHandlersFactory.GetMouseSelectionHandler(cameraController, buildingNameHover, buildingSelector);

        private ToggleCheatMenuInputHandler GetToggleCheatMenuInputHandler(UIManager uiManager) =>
            _inputHandlersFactory.GetToggleCheatMenuInputHandler(uiManager);

        private ToggleSystemPanelInputHandler GetToggleSystemPanelInputHandler(UIManager uiManager) =>
            _inputHandlersFactory.GetToggleSystemPanelInputHandler(uiManager);

        public void AddToggleCheatMenuInputHandlerToGlobal(UIManager uiManager)
        {
            foreach (InputHandler globalInputHandler in _globalInputHandlers)
            {
                if (globalInputHandler is ToggleCheatMenuInputHandler)
                {
                    Debug.LogError(
                        $"Trying to add global handler of type {nameof(ToggleCheatMenuInputHandler)}, but there is already one active");
                    return;
                }
            }

            _globalInputHandlers.Add(GetToggleCheatMenuInputHandler(uiManager));
        }

        public void AddToggleSystemPanelInputHandlerToGlobal(UIManager uiManager)
        {
            foreach (InputHandler globalInputHandler in _globalInputHandlers)
            {
                if (globalInputHandler is ToggleSystemPanelInputHandler)
                {
                    Debug.LogError(
                        $"Trying to add global handler of type {nameof(ToggleSystemPanelInputHandler)}, but there is already one active");
                    return;
                }
            }

            _globalInputHandlers.Add(GetToggleSystemPanelInputHandler(uiManager));
        }

        public void AddGlobalInputHandler<T>() where T : InputHandler, new()
        {
            foreach (InputHandler globalInputHandler in _globalInputHandlers)
            {
                if (globalInputHandler.GetType() == typeof(T))
                {
                    Debug.LogError(
                        $"Trying to add global handler of type {typeof(T)}, but there is already one active");
                    return;
                }
            }

            var handler = _inputHandlersFactory.GetInputHandler<T>();
            _globalInputHandlers.Add(handler);
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            foreach (InputHandler globalInputHandler in _globalInputHandlers)
            {
                globalInputHandler.HandleInput();
            }

            _onHandleInput.OnNext(Unit.Default);
        }
    }
}