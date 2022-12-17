#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.GameStates.UI
{
    public sealed class ExpeditionResultsPanel : UIElement<ExpeditionResult>
    {
        private GameStateMachine _gameStateMachine = null!;
        private ExpeditionResult _expeditionResult;

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

        protected override void OnShow(ExpeditionResult expeditionResult)
        {
            _expeditionResult = expeditionResult;
            if (_expeditionResult.IsSuccess)
            {
                _successPanel.Show(expeditionResult);
                _failPanel.Hide();
            }
            else
            {
                _failPanel.Show(expeditionResult);
                _successPanel.Hide();
            }
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