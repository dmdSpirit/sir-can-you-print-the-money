#nullable enable
using System;

namespace NovemberProject.Cheats
{
    public readonly struct CheatButtonInfo
    {
        public readonly string Title;
        public readonly Action Action;

        public CheatButtonInfo(string title, Action action)
        {
            Title = title;
            Action = action;
        }
    }
}