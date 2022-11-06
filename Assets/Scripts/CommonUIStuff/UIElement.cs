#nullable enable
using System;
using UniRx;

namespace NovemberProject.CommonUIStuff
{
    public abstract class UIElement : InitializableBehaviour
    {
        private readonly Subject<UIElement> _onShown = new();
        private readonly Subject<UIElement> _onHidden = new();

        public IObservable<UIElement> OnShown => _onShown;
        public IObservable<UIElement> OnHidden => _onHidden;

        public bool IsShown { get; private set; }

        public void Show()
        {
            OnShow();
            IsShown = true;
            _onShown.OnNext(this);
        }

        public void Hide()
        {
            OnHide();
            IsShown = false;
            _onHidden.OnNext(this);
        }

        protected abstract void OnShow();
        protected abstract void OnHide();
    }
}