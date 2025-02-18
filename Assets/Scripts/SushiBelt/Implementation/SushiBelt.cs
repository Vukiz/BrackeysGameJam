using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Orders;
using SushiBelt.Infrastructure;
using SushiBelt.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SushiBelt.Implementation
{
    public class SushiBelt : ISushiBelt
    {
        public event Action<IOrder> OrderReceived;
        public event Action<IOrder> OrderCompleted;
        public event Action<IOrder> OrderExpired;
        public IOrder CurrentOrder { get; private set; }
        private GameObject _currentOrderGameObject;

        private SushiBeltView _sushiBeltView;

        public void SubmitOrder(IOrder order)
        {
            Debug.Log($"Order received on the sushi belt. {order.NeededTypes.Count} types needed.");
            CurrentOrder = order;
            CreateOrder(order);
            order.OrderCompleted += OnOrderCompleted;
            order.TimerExpired += OnOrderExpired;
            OrderReceived?.Invoke(order);
            
            MoveOrderToTarget();
        }

        private void CreateOrder(IOrder order)
        {
            _currentOrderGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _currentOrderGameObject.transform.position = _sushiBeltView.StartPoint.position;
            _currentOrderGameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        private void MoveOrderToTarget()
        {
            _currentOrderGameObject.transform.DOMove(_sushiBeltView.TargetPoint.position, 1f).SetEase(Ease.Linear);
        }

        public void SetView(SushiBeltView sushiBeltView)
        {
            _sushiBeltView = sushiBeltView;
        }

        private void OnOrderCompleted()
        {
            var order = CurrentOrder;
            UnsubscribeOrder();

            MoveAwayOrder().Forget();
            OrderCompleted?.Invoke(order);
        }
        

        private void OnOrderExpired()
        {
            var order = CurrentOrder;
            UnsubscribeOrder();

            MoveAwayOrder().Forget();
            OrderExpired?.Invoke(order);
        }

        private void UnsubscribeOrder()
        {
            var order = CurrentOrder;
            order.OrderCompleted -= OnOrderCompleted;
            order.TimerExpired -= OnOrderExpired;
        }
        
        private async UniTaskVoid MoveAwayOrder()
        {
            await _currentOrderGameObject.transform.DOMove(_sushiBeltView.EndPoint.position, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
               Object.Destroy(_currentOrderGameObject);
            });
            Debug.Log("Order Completed and moved away from the sushi belt.");
            CurrentOrder = null;
        }
    }
}