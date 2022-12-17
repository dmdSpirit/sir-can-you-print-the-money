#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.GameStates.UI
{
    public sealed class AttackResultsPanel : UIElement<AttackData>
    {
        private GameStateMachine _gameStateMachine = null!;
        private AttackData _attackData;

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

        protected override void OnShow(AttackData attackData)
        {
            if (attackData.AttackStatus == AttackStatus.DefendersWon)
            {
                _defendersWonPanel.Show(attackData);
            }
            else
            {
                _defendersWonPanel.Hide();
            }

            if (attackData.AttackStatus == AttackStatus.GuardsKilled)
            {
                _guardsKilledPanel.Show(attackData);
            }
            else
            {
                _guardsKilledPanel.Hide();
            }

            if (attackData.AttackStatus == AttackStatus.FolkKilled)
            {
                _folkKilledPanel.Show(attackData);
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

        private void OnClose(Unit _)
        {
            _gameStateMachine.HideAttackResult();
        }
    }
}