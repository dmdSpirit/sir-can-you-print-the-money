#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public sealed class RoundEndPanel : UIElement<RoundResult>
    {
        private GameStateMachine _gameStateMachine = null!;

        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private string _titleText = null!;

        [SerializeField]
        private Button _nextRound = null!;

        [SerializeField]
        private RoundResultPanel _roundResultPanel = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _nextRound.OnClickAsObservable()
                .Subscribe(OnNextRound);
        }

        protected override void OnShow(RoundResult roundResult)
        {
            if (roundResult.NothingHappened())
            {
                _gameStateMachine.StartRound();
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
            _gameStateMachine.StartRound();
        }
    }
}