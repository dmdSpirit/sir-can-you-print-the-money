#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.GameStates.UI
{
    public interface IAttackResultsPanel : IUIScreen
    {
        public void SetAttackData(AttackData attackData);
    }

    public sealed class AttackResultsPanel : UIScreen, IAttackResultsPanel
    {
        private GameStateMachine _gameStateMachine = null!;
        private AttackData? _attackData;

        [SerializeField]
        private AttackResultPanel _defendersWonPanel = null!;

        [SerializeField]
        private AttackResultPanel _guardsKilledPanel = null!;

        [SerializeField]
        private AttackResultPanel _folkKilledPanel = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _defendersWonPanel.OnClose.Subscribe(OnClose);
            _guardsKilledPanel.OnClose.Subscribe(OnClose);
            _folkKilledPanel.OnClose.Subscribe(OnClose);
        }

        protected override void OnShow()
        {
            if (_attackData == null)
            {
                Debug.LogError($"{nameof(_attackData)} should be set before showing {nameof(AttackResultsPanel)}.");
                return;
            }
            
            if (_attackData.Value.AttackStatus == AttackStatus.DefendersWon)
            {
                _defendersWonPanel.Show(_attackData.Value);
            }
            else
            {
                _defendersWonPanel.Hide();
            }

            if (_attackData.Value.AttackStatus == AttackStatus.GuardsKilled)
            {
                _guardsKilledPanel.Show(_attackData.Value);
            }
            else
            {
                _guardsKilledPanel.Hide();
            }

            if (_attackData.Value.AttackStatus == AttackStatus.FolkKilled)
            {
                _folkKilledPanel.Show(_attackData.Value);
            }
            else
            {
                _folkKilledPanel.Hide();
            }
        }

        protected override void OnHide()
        {
            _defendersWonPanel.Hide();
            _guardsKilledPanel.Hide();
            _folkKilledPanel.Hide();
        }
        
        public void SetAttackData(AttackData attackData)
        {
            _attackData = attackData;
        }

        private void OnClose(Unit _)
        {
            _gameStateMachine.HideAttackResult();
        }
    }
}