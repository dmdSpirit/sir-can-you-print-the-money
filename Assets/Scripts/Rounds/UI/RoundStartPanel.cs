#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public sealed class RoundStartPanel : UIElement<object?>
    {
        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private string _titleText = null!;

        [SerializeField]
        private Button _nextRound = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _nextRound.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNextRound);
        }

        protected override void OnShow(object? _)
        {
            _title.text = _titleText.Replace("[value]", Game.Instance.RoundSystem.Round.Value.ToString());
        }

        protected override void OnHide()
        {
        }

        private void OnNextRound(Unit _)
        {
            Game.Instance.GameStateMachine.Round();
        }
    }
}