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
    public sealed class RoundStartPanel : UIElement<RoundStartResult>
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
        private string _desertedArmyText = null!;

        [SerializeField]
        private TMP_Text _desertedArmy = null!;

        [SerializeField]
        private RoundStartConfigs _roundStartConfigs = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _nextRound.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNextRound);
        }

        protected override void OnShow(RoundStartResult roundStartResult)
        {
            int round = Game.Instance.RoundSystem.Round.Value;
            RoundStartConfig config = _roundStartConfigs.GetRoundStartConfig(round);
            _title.text = config.Title;
            _description.text = config.Description;
            _image.sprite = config.Image;
            if (roundStartResult.ArmyDeserted != 0)
            {
                _desertedArmy.gameObject.SetActive(true);
                _desertedArmy.text = $"{_desertedArmyText}: {roundStartResult.ArmyDeserted}";
            }
            else
            {
                _desertedArmy.gameObject.SetActive(false);
            }
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