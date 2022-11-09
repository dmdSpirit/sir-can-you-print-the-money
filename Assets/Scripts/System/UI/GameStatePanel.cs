#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.System.UI
{
    public class GameStatePanel : UIElement<object?>
    {
        private IDisposable? _sub;

        [SerializeField]
        private TMP_Text _stateName = null!;

        protected override void OnShow(object? _)
        {
            _sub = Game.Instance.OnStateChanged.Subscribe(UpdateState);
        }

        protected override void OnHide()
        {
            _sub?.Dispose();
        }

        private void UpdateState(State state)
        {
            _stateName.text = state.GetType().Name.Replace("State", "");
        }
    }
}