#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.Buildings
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class AudioManager : InitializableBehaviour
    {
        private AudioSource _audioSource;
        
        [SerializeField]
        private AudioClip[] _clickClips = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandom()
        {
            AudioClip clip = _clickClips[Random.Range(0, _clickClips.Length)];
            _audioSource.PlayOneShot(clip, 0.7f);
        }
    }
}