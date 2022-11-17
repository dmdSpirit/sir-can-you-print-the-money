#nullable enable
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.CommonUIStuff
{
    [RequireComponent(typeof(Button))]
    public sealed class SelectableButton : MonoBehaviour
    {
        private readonly Subject<Unit> _onClicked = new();
        private Color _unselectedColor;

        [SerializeField]
        private Color _selectedColor;

        [SerializeField]
        private Image _targetGraghic;

        public Button Button { get; private set; } = null!;
        public IObservable<Unit> OnClicked => _onClicked;

        public bool IsSelected { get; private set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
            _unselectedColor = _targetGraghic.color;
            Button.OnClickAsObservable().Subscribe(OnButtonClicked);
        }

        public void Select()
        {
            if (IsSelected)
            {
                return;
            }

            _targetGraghic.color = _selectedColor;
            IsSelected = true;
        }

        public void Unselect()
        {
            if (!IsSelected)
            {
                return;
            }

            _targetGraghic.color = _unselectedColor;
            IsSelected = false;
        }

        private void OnButtonClicked(Unit _)
        {
            // Select();
            _onClicked.OnNext(Unit.Default);
        }
    }
}