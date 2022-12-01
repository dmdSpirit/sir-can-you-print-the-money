#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.GameStates.UI
{
    public sealed class AttackResultPanel : UIElement<AttackData>
    {
        private AttackData _attackData;

        [SerializeField]
        private TMP_Text _attackersCount = null!;

        [SerializeField]
        private TMP_Text? _defendersCount;

        [SerializeField]
        private Button _closeButton = null!;

        public IObservable<Unit> OnClose => _closeButton.OnClickAsObservable();

        protected override void OnShow(AttackData attackData)
        {
            _attackersCount.text = attackData.Attackers.ToString();
            if (_defendersCount != null)
            {
                _defendersCount.text = attackData.Defenders.ToString();
            }
        }

        protected override void OnHide()
        {
        }
    }
}