using TMPro;
using UnityEngine;

namespace Tutorial.Views
{
    public class TutorialDialogView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}