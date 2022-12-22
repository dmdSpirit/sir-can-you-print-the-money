#nullable enable
using System.Collections.Generic;
using System.Linq;
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;

namespace NovemberProject.System.UI
{
    [RequireComponent(typeof(MouseOverObserver))]
    public sealed class UIManager : MonoBehaviour
    {
        private readonly List<IUIScreen> _screens = new();

        private MouseOverObserver _mouseOverObserver = null!;

        [SerializeField]
        private LayerMask _layerMask;

        public LayerMask LayerMask => _layerMask;
        public IReadOnlyReactiveProperty<bool> IsMouseOver => _mouseOverObserver.IsMouseOver;

        private void Awake()
        {
            _mouseOverObserver = GetComponent<MouseOverObserver>();
        }

        public void Register(IUIScreen screen)
        {
            _screens.Add(screen);
            screen.Hide();
        }

        public T GetScreen<T>() where T : IUIScreen => (T)_screens.First(screen => screen is T);

        public void HideAll()
        {
            foreach (IUIScreen uiScreen in _screens)
            {
                uiScreen.Hide();
            }
        }
    }
}