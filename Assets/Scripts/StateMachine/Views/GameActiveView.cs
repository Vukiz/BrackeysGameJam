using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class GameActiveView : BaseView
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _muteButton;
        [SerializeField] private TMP_Text _ordersText;
        [SerializeField] private Slider _ordersSlider;
        [SerializeField] private Slider _failedOrdersSlider;
        [SerializeField] private AudioSource _winAudio;
        [SerializeField] private AudioSource _loseAudio;

        
        public AudioSource WinAudio => _winAudio;
        public AudioSource LoseAudio => _loseAudio;
        public Button ExitButton => _exitButton;
        public Button MuteButton => _muteButton;
        public TMP_Text OrdersText => _ordersText;
        public Slider OrdersSlider => _ordersSlider;
        public Slider FailedOrdersSlider => _failedOrdersSlider;
    }
}