#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public sealed class TechTreeButton : InitializableBehaviour
    {
        private GameStateMachine _gameStateMachine = null!;

        [SerializeField]
        private Button _button = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _button.OnClickAsObservable()
                .Subscribe(OnButton);
        }

        private void OnButton(Unit _)
        {
            if (_gameStateMachine.CurrentState is not RoundState)
            {
                return;
            }

            _gameStateMachine.ShowTechTree();
        }
    }
}