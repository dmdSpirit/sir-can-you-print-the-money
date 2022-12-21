#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public interface IRoundEndPanel : IUIScreen
    {
        public void SetRoundResult(RoundResult roundResult);
    }

    public sealed class RoundEndPanel : UIScreen, IRoundEndPanel
    {
        private GameStateMachine _gameStateMachine = null!;
        private RoundSystem _roundSystem = null!;

        private RoundResult? _roundResult;

        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private string _titleText = null!;

        [SerializeField]
        private Button _nextRound = null!;

        [SerializeField]
        private RoundResultPanel _roundResultPanel = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, RoundSystem roundSystem)
        {
            _gameStateMachine = gameStateMachine;
            _roundSystem = roundSystem;
        }

        private void Start()
        {
            _nextRound.OnClickAsObservable()
                .Subscribe(OnNextRound);
        }

        protected override void OnShow()
        {
            if (_roundResult == null)
            {
                Debug.LogError($"{nameof(_roundResult)} should be set before showing {nameof(RoundEndPanel)}.");
                return;
            }

            if (_roundResult.NothingHappened())
            {
                _gameStateMachine.StartRound();
                return;
            }

            _title.text = _titleText.Replace("[value]", _roundSystem.Round.Value.ToString());
            _roundResultPanel.Show(_roundResult);
        }

        protected override void OnHide()
        {
            _roundResult = null;
            _roundResultPanel.Hide();
        }

        public void SetRoundResult(RoundResult roundResult)
        {
            _roundResult = roundResult;
        }

        private void OnNextRound(Unit _)
        {
            _gameStateMachine.StartRound();
        }
    }
}