#nullable enable
using System;
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
        private TMP_Text _starvedFolk = null!;
        [SerializeField]
        private TMP_Text _starvedArmy = null!;
        [SerializeField]
        private TMP_Text _executedFolk = null!;
        
        protected override void OnShow(RoundResult roundResult)
        {
            if (roundResult.FolkStarved != 0)
            {
                _starvedFolk.text = $"{_starvedFolkText}: {roundResult.FolkStarved}";
                _starvedFolk.gameObject.SetActive(true);
            }
            else
            {
                _starvedFolk.gameObject.SetActive(false);
            }            
            if (roundResult.ArmyStarved != 0)
            {
                _starvedArmy.text = $"{_starvedArmyText}: {roundResult.ArmyStarved}";
                _starvedArmy.gameObject.SetActive(true);
            }
            else
            {
                _starvedArmy.gameObject.SetActive(false);
            }            
            if (roundResult.FolkExecuted != 0)
            {
                _executedFolk.text = $"{_executedFolkText}: {roundResult.FolkExecuted}";
                _executedFolk.gameObject.SetActive(true);
            }
            else
            {
                _executedFolk.gameObject.SetActive(false);
            }
        }

        protected override void OnHide()
        {
        }
    }
}