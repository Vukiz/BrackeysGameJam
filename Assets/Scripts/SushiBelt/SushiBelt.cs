using System;
using Orders;

namespace SushiBelt
{
    public class SushiBelt : ISushiBelt
    {
        public event Action<IOrder> OrderReceived;
        public event Action<IOrder> OrderCompleted;
        public event Action<IOrder> OrderExpired;
        public IOrder CurrentOrder { get; private set; }

        private SushiBeltView _sushiBeltView;

        public void SubmitOrder(IOrder order)
        {
            CurrentOrder = order;
            order.OrderCompleted += OnOrderCompleted;
            order.TimerExpired += OnOrderExpired;
            OrderReceived?.Invoke(order);
        }

        public void SetView(SushiBeltView sushiBeltView)
        {
            _sushiBeltView = sushiBeltView;
        }

        private void OnOrderCompleted()
        {
            var order = CurrentOrder;
            UnsubscribeOrder();

            MoveAwayOrder();
            OrderCompleted?.Invoke(order);
        }

        private void OnOrderExpired()
        {
            var order = CurrentOrder;
            UnsubscribeOrder();

            MoveAwayOrder();
            OrderExpired?.Invoke(order);
        }

        private void UnsubscribeOrder()
        {
            var order = CurrentOrder;
            order.OrderCompleted -= OnOrderCompleted;
            order.TimerExpired -= OnOrderExpired;
        }
        
        private void MoveAwayOrder()
        {
            // TODO Move away the order
            CurrentOrder = null;
        }
    }
}