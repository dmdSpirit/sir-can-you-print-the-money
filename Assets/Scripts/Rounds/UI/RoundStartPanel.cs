#nullable enable
using NovemberProject.CommonUIStuff;
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
        private string _titleText = null!;

        [SerializeField]
        private Button _nextRound = null!;

        [SerializeField]
        private string _desertedArmyText = null!;
        
        [SerializeField]
        private TMP_Text _desertedArmy = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _nextRound.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNextRound);
        }

        protected override void OnShow(RoundStartResult roundStartResult)
        {
            _title.text = _titleText.Replace("[value]", Game.Instance.RoundSystem.Round.Value.ToString());
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