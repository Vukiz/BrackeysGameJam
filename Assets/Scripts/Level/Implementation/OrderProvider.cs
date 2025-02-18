using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Level.Infrastructure;
using Orders;
using SushiBelt.Infrastructure;
using UnityEngine;

namespace Level.Implementation
{
    public class OrderProvider : IOrderProvider
    {
        public event Action LevelCompleted;

        private readonly List<ISushiBelt> _sushiBelts = new();
        private readonly Queue<IOrder> _ordersToComplete = new();
        private readonly List<IOrder> _completedOrders = new();
        private readonly List<IOrder> _expiredOrders = new();
        private int totalOrdersCount;
        
        private CancellationTokenSource _cancellationTokenSource;

        public void Initialize(IEnumerable<IOrder> orders)
        {
            foreach (var sushiBelt in _sushiBelts)
            {
                sushiBelt.OrderCompleted -= OnOrderCompleted;
                sushiBelt.OrderExpired -= OnOrderExpired;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _sushiBelts.Clear();
            _ordersToComplete.Clear();
            _completedOrders.Clear();
            _expiredOrders.Clear();

            foreach (var order in orders)
            {
                _ordersToComplete.Enqueue(order);
            }
            
            Debug.Log($"Initialized with {_ordersToComplete.Count} orders");
            totalOrdersCount = _ordersToComplete.Count;
        }

        public void RegisterSushiBelt(ISushiBelt sushiBelt)
        {
            sushiBelt.OrderCompleted += OnOrderCompleted;
            sushiBelt.OrderExpired += OnOrderExpired;
            _sushiBelts.Add(sushiBelt);
        }

        public async UniTaskVoid StartProcessingOrders()
        {
            while (!_cancellationTokenSource.IsCancellationRequested && _ordersToComplete.Count > 0)
            {
                await UniTask.Delay(GetNextOrderDelay(), cancellationToken: _cancellationTokenSource.Token); 
                var sushiBelt = GetRandomEmptySushiBelt();
                sushiBelt?.SubmitOrder(_ordersToComplete.Dequeue());
            }
        }
        
        private TimeSpan GetNextOrderDelay()
        {
            return TimeSpan.FromSeconds(UnityEngine.Random.Range(1, 3));// TODO refactor this logic to be fun
        }

        private ISushiBelt GetRandomEmptySushiBelt()
        {
            var emptySushiBelts = _sushiBelts.Where(s => s.CurrentOrder == null).ToList();
            if (emptySushiBelts.Count == 0)
            {
                return null;
            }

            var randomIndex = UnityEngine.Random.Range(0, emptySushiBelts.Count);
            return emptySushiBelts.ElementAt(randomIndex);
        }

        public int GetFailedOrdersCount()
        {
            return _expiredOrders.Count;
        }

        public int GetCompletedOrdersCount()
        {
            return _completedOrders.Count;
        }

        public bool IsLevelWon()
        {
            return IsLevelCompleted();
        }

        private void OnOrderExpired(IOrder order)
        {
            _expiredOrders.Add(order);

            if (IsLevelCompleted())
            {
                CompleteLevel();
            }
        }

        private void OnOrderCompleted(IOrder order)
        {
            _completedOrders.Add(order);

            if (IsLevelCompleted())
            {
                CompleteLevel();
            }
        }

        private bool IsLevelCompleted()
        {
            return _ordersToComplete.Count == 0 && _expiredOrders.Count + _completedOrders.Count == totalOrdersCount;
        }

        private void CompleteLevel()
        {
            Debug.Log("Level completed, no more orders");
            LevelCompleted?.Invoke();
        }
    }
}