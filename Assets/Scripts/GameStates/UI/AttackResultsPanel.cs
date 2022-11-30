#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using Observable = UniRx.Observable;

namespace NovemberProject.GameStates.UI
{
    public sealed class AttackResultsPanel : UIElement<AttackData>
    {
        private AttackData _attackData;

        [SerializeField]
        private AttackResultPanel _defendersWonPanel = null!;
        [SerializeField]
        private AttackResultPanel _guardsKilledPanel = null!;
        [SerializeField]
        private AttackResultPanel _folkKilledPanel = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Observable.TakeUntilDisable(_defendersWonPanel.OnClose, this)
                .Subscribe(OnClose);
            Observable.TakeUntilDisable(_guardsKilledPanel.OnClose, this)
                .Subscribe(OnClose);
            Observable.TakeUntilDisable(_folkKilledPanel.OnClose, this)
                .Subscribe(OnClose);
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
            Game.Instance.GameStateMachine.HideAttackResult();
        }
    }
}