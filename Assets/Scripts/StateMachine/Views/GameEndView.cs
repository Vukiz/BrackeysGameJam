using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class GameEndView : MonoBehaviour
    {
        [SerializeField] private GameObject _starsContainer; // TODO replace with stars view
        [SerializeField] private Button _nextLevelButton;
        
        public GameObject StarsContainer => _starsContainer;
        public Button NextLevelButton => _nextLevelButton;
    }
}