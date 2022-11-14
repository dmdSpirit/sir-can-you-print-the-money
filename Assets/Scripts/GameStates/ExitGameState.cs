#nullable enable
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NovemberProject.GameStates
{
    public sealed class ExitGameState : State
    {
        protected override void OnEnter()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        protected override void OnExit()
        {
        }
    }
}