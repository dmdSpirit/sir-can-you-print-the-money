#nullable enable
using UnityEngine;

namespace NovemberProject.CommonUIStuff
{
    public class SimpleHideObjectHandler : MonoBehaviour, IShowTransitionHandler
    {
        public void OnShow()
        {
            gameObject.SetActive(false);
        }
    }
    
    public interface IHideTransitionHandler
    {
        public void OnHide();
    }
}