#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.ClicheSpeech.UI
{
    [RequireComponent(typeof(ContentSizeFitter))]
    public class SpeechBubble : UIElement<string>
    {
        private ContentSizeFitter? _contentSizeFitter;

        [SerializeField]
        private TMP_Text _text = null!;

        private void Awake()
        {
            _contentSizeFitter = GetComponent<ContentSizeFitter>();
        }

        protected override void OnShow(string text)
        {
            gameObject.SetActive(true);
            _text.text = text;
            Observable.IntervalFrame(1).Subscribe(_ => UpdateSize());
        }

        protected override void OnHide()
        {
            gameObject.SetActive(false);
            _text.text = string.Empty;
        }

        private void UpdateSize()
        {
            if (_contentSizeFitter == null)
            {
                return;
            }

            _contentSizeFitter.enabled = false;
            // ReSharper disable once Unity.InefficientPropertyAccess
            _contentSizeFitter.enabled = true;
        }
    }
}