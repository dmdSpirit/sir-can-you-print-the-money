#nullable enable
using UnityEngine;

namespace NovemberProject.CommonUIStuff
{
    public class SimpleHideObjectHandler : MonoBehaviour, IHideTransitionHandler
    {
        public void OnHide()
        {
            gameObject.SetActive(false);
        }
    }
}