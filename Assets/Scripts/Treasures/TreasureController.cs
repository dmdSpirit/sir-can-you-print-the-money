#nullable enable
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;

namespace NovemberProject.Treasures
{
    public sealed class TreasureController : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _treasures = new();

        [SerializeField]
        private int _startingTreasures = 0;

        public IReadOnlyReactiveProperty<int> Treasures => _treasures;

        public void InitializeGameData()
        {
            _treasures.Value = _startingTreasures;
        }
        
        
    }
}