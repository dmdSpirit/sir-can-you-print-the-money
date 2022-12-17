#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.System.UI
{
    public sealed class GameStatePanel : UIElement<object?>
    {
        private IDisposable? _sub;
        private GameStateMachine _gameStateMachine = null!;

        [SerializeField]
        private TMP_Text _stateName = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        protected override void OnShow(object? _)
        {
            _sub = _gameStateMachine.OnStateChanged.Subscribe(UpdateState);
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