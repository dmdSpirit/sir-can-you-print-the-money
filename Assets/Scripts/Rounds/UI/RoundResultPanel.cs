#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UnityEngine;

namespace NovemberProject.Rounds.UI
{
    public sealed class RoundResultPanel : UIElement<RoundResult>
    {
        [SerializeField]
        private string _starvedFolkText = "Starved folk";
        [SerializeField]
        private string _starvedArmyText = "Starved army";
        [SerializeField]
        private string _executedFolkText = "Executed folk";
        [SerializeField]
        private string _armyDesertedText = "Army deserted";
        
        [SerializeField]
        private TMP_Text _starvedFolk = null!;
        [SerializeField]
        private TMP_Text _starvedArmy = null!;
        [SerializeField]
        private TMP_Text _executedFolk = null!;
        [SerializeField]
        private TMP_Text _desertedArmy = null!;
        
        protected override void OnShow(RoundResult roundResult)
        {
                _starvedFolk.text = $"{_starvedFolkText} {roundResult.FolkStarved}";
                _starvedArmy.text = $"{_starvedArmyText} {roundResult.ArmyStarved}";
                _executedFolk.text = $"{_executedFolkText} {roundResult.FolkExecuted}";
                _desertedArmy.text = $"{_armyDesertedText} {roundResult.ArmyDeserted}";
        }

        protected override void OnHide()
        {
        }
    }
}