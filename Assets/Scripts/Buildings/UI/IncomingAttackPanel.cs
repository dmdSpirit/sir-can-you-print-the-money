#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings.UI
{
    public sealed class IncomingAttackPanel : UIElement<IIncomingAttack>
    {
        private readonly CompositeDisposable _sub = new();

        private TimeSystem _timeSystem = null!;
        private IIncomingAttack _incomingAttack = null!;

        [SerializeField]
        private TMP_Text _attackersText = null!;

        [SerializeField]
        private TMP_Text _winProbability = null!;

        [SerializeField]
        private TMP_Text _timeLeft = null!;

        [Inject]
        private void Construct(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        private void Update()
        {
            if (_incomingAttack.AttackTimer == null)
            {
                return;
            }

            _timeLeft.text = _timeSystem.EstimateSecondsLeftUnscaled(_incomingAttack.AttackTimer) + "s";
        }

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

        private void UpdateInfo()
        {
            _attackersText.text = _incomingAttack.Attackers.ToString();
            _winProbability.text = $"{(int)(_incomingAttack.WinProbability * 100)}%";
        }
    }
}