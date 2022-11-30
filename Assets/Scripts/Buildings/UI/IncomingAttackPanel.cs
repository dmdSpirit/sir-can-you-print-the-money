#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings.UI
{
    public sealed class IncomingAttackPanel : UIElement<IIncomingAttack>
    {
        private readonly CompositeDisposable _sub = new();
        private IIncomingAttack _incomingAttack = null!;

        [SerializeField]
        private TMP_Text _attackersText = null!;

        [SerializeField]
        private TMP_Text _winProbability = null!;

        [SerializeField]
        private TMP_Text _timeLeft = null!;

        protected override void OnShow(IIncomingAttack incomingAttack)
        {
            _sub.Clear();
            _incomingAttack = incomingAttack;
            _incomingAttack.Defenders.Subscribe(_ => UpdateInfo()).AddTo(_sub);
            _incomingAttack.OnNewAttack.Subscribe(_ => UpdateInfo()).AddTo(_sub);
        }

        protected override void OnHide()
        {
            _sub.Clear();
            _incomingAttack = null!;
        }

        private void Update()
        {
            if (_incomingAttack.AttackTimer == null)
            {
                return;
            }

            _timeLeft.text = Game.Instance.TimeSystem.EstimateSecondsLeftUnscaled(_incomingAttack.AttackTimer) + "s";
        }

        private void UpdateInfo()
        {
            _attackersText.text = _incomingAttack.Attackers.ToString();
            _winProbability.text = $"{(int)(_incomingAttack.WinProbability * 100)}%";
        }
    }
}