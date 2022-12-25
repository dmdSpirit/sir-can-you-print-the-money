#nullable enable
using System;
using UnityEngine;

namespace NovemberProject.Core
{
    [Serializable]
    public sealed class RoundStartConfig
    {
        public string Title = null!;
        public string Description = null!;
        public Sprite Image = null!;
    }
}