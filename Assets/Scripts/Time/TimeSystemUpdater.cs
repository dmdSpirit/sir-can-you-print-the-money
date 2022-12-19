#nullable enable
using UnityEngine;
using Zenject;

namespace NovemberProject.Time
{
    public sealed class TimeSystemUpdater : MonoBehaviour
    {
        private TimeSystem _timeSystem = null!;

        [Inject]
        private void Construct(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        private void Update()
        {
            _timeSystem.Update(UnityEngine.Time.deltaTime);
        }
    }
}