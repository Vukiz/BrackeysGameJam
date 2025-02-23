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

        public Button ExitButton => _exitButton;
        public Button MuteButton => _muteButton;
        public TMP_Text OrdersText => _ordersText;
        public Slider OrdersSlider => _ordersSlider;
        public Slider FailedOrdersSlider => _failedOrdersSlider;
    }
}