using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StateMachine.Views
{
    public abstract class BaseView : MonoBehaviour
    {
        public virtual UniTask Show()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }
        
        public virtual UniTask Hide()
        {
            if (!gameObject)
            {
                return UniTask.CompletedTask;
            }
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}