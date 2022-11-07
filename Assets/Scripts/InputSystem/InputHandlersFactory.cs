#nullable enable
using System;
using System.Collections.Generic;

namespace NovemberProject.InputSystem
{
    public class InputHandlersFactory
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
    }
}