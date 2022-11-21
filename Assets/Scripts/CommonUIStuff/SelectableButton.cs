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
        private Button _button = null!;
        private bool _isSelected;

        [SerializeField]
        private Color _selectedColor;

        [SerializeField]
        private Image _targetGraphic = null!;

        public IObservable<Unit> OnClicked => _onClicked;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _unselectedColor = _targetGraphic.color;
            _button.OnClickAsObservable().Subscribe(OnButtonClicked);
        }

        public void Select()
        {
            if (_isSelected)
            {
                return;
            }

            _targetGraphic.color = _selectedColor;
            _isSelected = true;
        }

        public void Unselect()
        {
            if (!_isSelected)
            {
                return;
            }

            _targetGraphic.color = _unselectedColor;
            _isSelected = false;
        }

        private void OnButtonClicked(Unit _)
        {
            _onClicked.OnNext(Unit.Default);
        }
    }
}