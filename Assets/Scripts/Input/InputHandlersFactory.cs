#nullable enable
using System;
using System.Collections.Generic;
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
    }
}