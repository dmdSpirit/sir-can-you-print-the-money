#nullable enable
using System;
using UniRx;
using UnityEngine;

namespace NovemberProject.CommonUIStuff
{
    public sealed class RadioButtonGroup : MonoBehaviour
    {
        private readonly Subject<int> _onButtonClicked = new();
        private int _selectedButton;
        private bool _isAnythingSelected;
        private bool _isLocked;

        [SerializeField]
        private SelectableButton[] _buttons = null!;

        public IObservable<int> OnButtonClicked => _onButtonClicked;

        private void OnEnable()
        {
            for (var buttonIndex = 0; buttonIndex < _buttons.Length; buttonIndex++)
            {
                SelectableButton button = _buttons[buttonIndex];
                int clickedButtonIndex = buttonIndex;
                button.OnClicked
                    .TakeUntilDisable(this)
                    .Subscribe(_ => ButtonClickedHandler(clickedButtonIndex));
            }
        }

        public void Lock() => _isLocked = true;
        public void Unlock() => _isLocked = false;

        public void SetClickedButtonSilently(int index)
        {
            if (_isAnythingSelected)
            {
                _buttons[_selectedButton].Unselect();
            }
            else
            {
                _isAnythingSelected = true;
            }

            _buttons[index].Select();
            _selectedButton = index;
        }

        private void ButtonClickedHandler(int buttonIndex)
        {
            if (_isLocked)
            {
                return;
            }

            SetClickedButtonSilently(buttonIndex);
            _onButtonClicked.OnNext(buttonIndex);
        }
    }
}