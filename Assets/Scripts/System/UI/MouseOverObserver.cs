#nullable enable
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine.EventSystems;

namespace NovemberProject.System.UI
{
    public sealed class MouseOverObserver : InitializableBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private readonly ReactiveProperty<bool> _isMouseOver = new();

        public IReadOnlyReactiveProperty<bool> IsMouseOver => _isMouseOver;

        public void OnPointerEnter(PointerEventData eventData) => _isMouseOver.Value = true;
        public void OnPointerExit(PointerEventData eventData) => _isMouseOver.Value = false;
    }
}