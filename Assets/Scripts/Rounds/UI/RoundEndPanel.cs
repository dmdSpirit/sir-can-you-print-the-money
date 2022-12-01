#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public sealed class RoundEndPanel : UIElement<RoundResult>
    {
        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private string _titleText = null!;

        [SerializeField]
        private Button _nextRound = null!;

        [SerializeField]
        private RoundResultPanel _roundResultPanel = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _nextRound.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNextRound);
        }

        protected override void OnShow(RoundResult roundResult)
        {
            if (roundResult.NothingHappened())
            {
                Game.Instance.GameStateMachine.StartRound();
                return;
            }
            _title.text = _titleText.Replace("[value]", Game.Instance.RoundSystem.Round.Value.ToString());
            _roundResultPanel.Show(roundResult);
        }

        protected override void OnHide()
        {
            _roundResultPanel.Hide();
        }

        private void OnNextRound(Unit _)
        {
            Game.Instance.GameStateMachine.StartRound();
        }
    }
}