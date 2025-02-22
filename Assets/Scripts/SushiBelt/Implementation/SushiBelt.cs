using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Orders.Infrastructure;
using Orders.Views;
using SushiBelt.Infrastructure;
using SushiBelt.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SushiBelt.Implementation
{
    public class SushiBelt : ISushiBelt, IDisposable
    {
        public event Action<IOrder> OrderReceived;
        public event Action<IOrder> OrderCompleted;
        public event Action<IOrder> OrderExpired;

        public void Cleanup()
        {
            Dispose();
        }

        public IOrder CurrentOrder { get; private set; }
        public Vector3 OrderPosition => _currentOrderGameObject.transform.position;
        private OrderView _currentOrderGameObject;

        private SushiBeltView _sushiBeltView;

        public async void SubmitOrder(IOrder order)
        {
            Debug.Log($"Order received on the sushi belt. {order.NeededTypes.Count} types needed.");
            CreateOrder(order);

            await MoveOrderToTarget();
            CurrentOrder = order;
            order.OrderCompleted += OnOrderCompleted;
            order.TimerExpired += OnOrderExpired;
            OrderReceived?.Invoke(order);
        }

        private void CreateOrder(IOrder order)
        {
            _currentOrderGameObject = Object.Instantiate(_sushiBeltView.OrderViewPrefab);
            _currentOrderGameObject.transform.position = _sushiBeltView.StartPoint.position;
            SetupOrderView(_currentOrderGameObject, order);
        }

        private void SetupOrderView(OrderView orderView, IOrder order)
        {
            foreach (var orderViewWorkTypeToObjectPair in orderView.WorkTypeToObjectPairs)
            {
                orderViewWorkTypeToObjectPair.Object.SetActive(order.NeededTypes.Contains(orderViewWorkTypeToObjectPair.WorkType), order.Duration);
            }
        }

        private async UniTask MoveOrderToTarget()
        {
            await _currentOrderGameObject.transform.DOMove(_sushiBeltView.TargetPoint.position, 1f)
                .SetEase(Ease.Linear);
        }

        public void SetView(SushiBeltView sushiBeltView)
        {
            _sushiBeltView = sushiBeltView;
            _sushiBeltView.Destroyed += Dispose;
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
            if (order == null)
            {
                return;
            }

            order.OrderCompleted -= OnOrderCompleted;
            order.TimerExpired -= OnOrderExpired;
        }

        private async UniTaskVoid MoveAwayOrder()
        {
            _currentOrderGameObject.DisableOutline();
            await _currentOrderGameObject.transform.DOMove(_sushiBeltView.EndPoint.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() => { Object.Destroy(_currentOrderGameObject.gameObject); });
            Debug.Log("Order Completed and moved away from the sushi belt.");
            CurrentOrder = null;
        }

        public void Dispose()
        {
            UnsubscribeOrder();
            if (_sushiBeltView != null)
            {
                _sushiBeltView.Destroyed -= Dispose;
            }

            if (_currentOrderGameObject != null)
            {
                Object.Destroy(_currentOrderGameObject.gameObject);
            }
        }
    }
}