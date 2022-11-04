#nullable enable
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject
{
    [RequireComponent(typeof(ContentSizeFitter))]
    public class SpeechBubble : MonoBehaviour
    {
        private ContentSizeFitter? _contentSizeFitter;

        [SerializeField]
        private TMP_Text _text = null!;

        private void Awake()
        {
            _contentSizeFitter = GetComponent<ContentSizeFitter>();
        }

        public void Show(string text)
        {
            gameObject.SetActive(true);
            _text.text = text;
            Observable.IntervalFrame(1).Subscribe(_ => UpdateSize());
        }

        public void Hide()
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
            _contentSizeFitter.enabled = true;
        }
    }
}