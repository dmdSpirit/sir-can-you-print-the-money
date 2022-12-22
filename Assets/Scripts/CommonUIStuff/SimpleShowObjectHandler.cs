#nullable enable
using UnityEngine;

namespace NovemberProject.CommonUIStuff
{
    public class SimpleShowObjectHandler : MonoBehaviour, IShowTransitionHandler
    {
        public void OnShow()
        {
            gameObject.SetActive(true);
        }
    }
}