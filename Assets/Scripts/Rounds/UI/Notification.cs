#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UnityEngine;

namespace NovemberProject.Rounds.UI
{
    public sealed class Notification : UIElement<int>
    {
        [SerializeField]
        private TMP_Text _count = null!;
        
        protected override void OnShow(int value)
        {
            _count.text = value.ToString();
        }

        protected override void OnHide()
        {
        }
    }
}