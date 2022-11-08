#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public class EndOfRoundPanel : UIElement<object?>
    {
        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private string _titleText = null!;

        [SerializeField]
        private Button _nextRound = null!;

        protected override void Initialize()
        {
            _nextRound.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNextRound);
        }

        protected override void OnShow(object? _)
        {
            gameObject.SetActive(true);
            _title.text = _titleText.Replace("[value]", Game.Instance.RoundSystem.Round.Value.ToString());
        }

        protected override void OnHide()
        {
            gameObject.SetActive(false);
        }

        private void OnNextRound(Unit _)
        {
            Game.Instance.NextRound();
        }
    }
}