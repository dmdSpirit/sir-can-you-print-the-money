#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public sealed class TechTreeButton : InitializableBehaviour
    {
        [SerializeField]
        private Button _button = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _button.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnButton);
        }

        private void OnButton(Unit _)
        {
            if (Game.Instance.GameStateMachine.CurrentState is not RoundState)
            {
                return;
            }

            Game.Instance.GameStateMachine.ShowTechTree();
        }
    }
}