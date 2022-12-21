#nullable enable
using NovemberProject.System.UI;
using UnityEngine;
using Zenject;

namespace NovemberProject.CommonUIStuff
{
    public abstract class UIScreen : MonoBehaviour, IUIScreen
    {
        private bool _isShown;
        private IShowTransitionHandler[]? _showTransitionHandlers;
        private IHideTransitionHandler[]? _hideTransitionHandlers;

        public bool IsShown => _isShown;

        [Inject]
        protected void Register(UIManager uiManager)
        {
            _showTransitionHandlers = GetComponents<IShowTransitionHandler>();
            _hideTransitionHandlers = GetComponents<IHideTransitionHandler>();
            uiManager.Register(this);
        }

        public void Show()
        {
            if (_isShown)
            {
                Debug.LogWarning($"Trying to show {name} but it is already shown.");
            }

            _isShown = true;
            OnShow();
            HandleShowTransitions();
        }

        public void Hide()
        {
            _isShown = false;
            OnHide();
            HandleHideTransitions();
        }

        protected abstract void OnShow();
        protected abstract void OnHide();

        private void HandleShowTransitions()
        {
            if (_showTransitionHandlers == null)
            {
                return;
            }

            foreach (IShowTransitionHandler showTransitionHandler in _showTransitionHandlers)
            {
                showTransitionHandler.OnShow();
            }
        }

        private void HandleHideTransitions()
        {
            if (_hideTransitionHandlers == null)
            {
                return;
            }

            foreach (IHideTransitionHandler hideTransitionHandler in _hideTransitionHandlers)
            {
                hideTransitionHandler.OnHide();
            }
        }
    }
}