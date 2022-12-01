#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public sealed class RoundStartPanel : UIElement<object?>
    {
        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private TMP_Text _description = null!;

        [SerializeField]
        private Image _image = null!;

        [SerializeField]
        private Button _nextRound = null!;

        [SerializeField]
        private RoundStartConfigs _roundStartConfigs = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _nextRound.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNextRound);
        }

        protected override void OnShow(object? _)
        {
            int round = Game.Instance.RoundSystem.Round.Value;
            RoundStartConfig config = _roundStartConfigs.GetRoundStartConfig(round);
            _title.text = config.Title;
            _description.text = config.Description;
            _image.sprite = config.Image;
        }

        protected override void OnHide()
        {
        }

        private static void OnNextRound(Unit _)
        {
            Game.Instance.GameStateMachine.Round();
        }
    }
}