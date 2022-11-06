#nullable enable
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.CommonUIStuff
{
    [RequireComponent(typeof(Button))]
    public class SelectableButton : MonoBehaviour
    {
        private readonly Subject<Unit> _onClicked = new();

        [SerializeField]
        private Color _selectedColor;

        [SerializeField]
        private Color _unselectedColor;

        public Button Button { get; private set; } = null!;
        public IObservable<Unit> OnClicked => _onClicked;

        public bool IsSelected { get; private set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
            Button.OnClickAsObservable().Subscribe(OnButtonClicked);
        }

        private void OnButtonClicked(Unit _)
        {
            Select();
            _onClicked.OnNext(Unit.Default);
        }

        public void Select()
        {
            if (IsSelected)
            {
                return;
            }

            Button.targetGraphic.color = _selectedColor;
            IsSelected = true;
        }

        public void Unselect()
        {
            if (!IsSelected)
            {
                return;
            }

            Button.targetGraphic.color = _unselectedColor;
            IsSelected = false;
        }
    }
}