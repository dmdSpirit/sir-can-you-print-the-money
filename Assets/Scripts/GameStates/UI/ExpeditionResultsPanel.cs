#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.GameStates.UI
{
    public interface IExpeditionResultsPanel : IUIScreen
    {
        public void SetExpeditionResult(ExpeditionResult expeditionResult);
    }

    public sealed class ExpeditionResultsPanel : UIScreen, IExpeditionResultsPanel
    {
        private GameStateMachine _gameStateMachine = null!;
        private ExpeditionResult? _expeditionResult;

        [SerializeField]
        private ExpeditionResultPanel _successPanel = null!;

        [SerializeField]
        private ExpeditionResultPanel _failPanel = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _successPanel.OnClose
                .Subscribe(OnCloseClicked);
            _failPanel.OnClose
                .Subscribe(OnCloseClicked);
        }

        protected override void OnShow()
        {
            if (_expeditionResult == null)
            {
                Debug.LogError($"{nameof(_expeditionResult)} should be set before showing {nameof(ExpeditionResultPanel)}.");
                return;
            }

            if (_expeditionResult.Value.IsSuccess)
            {
                _successPanel.Show(_expeditionResult.Value);
                _failPanel.Hide();
            }
            else
            {
                _failPanel.Show(_expeditionResult.Value);
                _successPanel.Hide();
            }
        }
        
        public void SetExpeditionResult(ExpeditionResult expeditionResult)
        {
            _expeditionResult = expeditionResult;
        }

        protected override void OnHide()
        {
            _successPanel.Hide();
            _failPanel.Hide();
        }

        private void OnCloseClicked(Unit _)
        {
            _gameStateMachine.ExpeditionFinishedExit();
        }
    }
}