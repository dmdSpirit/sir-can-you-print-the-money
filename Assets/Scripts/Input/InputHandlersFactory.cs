#nullable enable
using System;
using System.Collections.Generic;
using NovemberProject.CameraSystem;
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

        public MouseSelectionHandler GetMouseSelectionHandler(CameraController cameraController)
        {
            if (!_inputHandlers.ContainsKey(typeof(MouseSelectionHandler)))
            {
                _inputHandlers.Add(typeof(MouseSelectionHandler), new MouseSelectionHandler(cameraController));
            }

            return (MouseSelectionHandler)_inputHandlers[typeof(MouseSelectionHandler)];
        }

        public MoveCameraHandler GetMoveCameraHandler(CameraController cameraController)
        {
            if (!_inputHandlers.ContainsKey(typeof(MoveCameraHandler)))
            {
                _inputHandlers.Add(typeof(MoveCameraHandler), new MoveCameraHandler(cameraController));
            }

            return (MoveCameraHandler)_inputHandlers[typeof(MoveCameraHandler)];
        }
    }
}