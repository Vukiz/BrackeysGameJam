using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Level.Data;
using Level.Infrastructure;
using Orders.Data;
using Orders.Implementation;
using Orders.Infrastructure;
using SushiBelt.Infrastructure;
using UnityEngine;

namespace Level.Implementation
{
    public class OrderProvider : IOrderProvider
    {
        private readonly RandomOrdersSettings _defaultRandomOrdersSettings = new()
        {
            OrdersCount = 10,
            TimeLimitSecondsMin = 15,
            TimeLimitSecondsMax = 25,
            WorkTypes = new List<WorkType>
            {
                WorkType.Paint,
                WorkType.Assemble,
                WorkType.Wield
            }
        };
        public event Action LevelCompleted;

        private readonly List<ISushiBelt> _sushiBelts = new();
        private readonly Queue<IOrder> _ordersToComplete = new();
        private readonly List<IOrder> _completedOrders = new();
        private readonly List<IOrder> _expiredOrders = new();
        private int _totalOrdersCount;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        private void Cleanup()
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
        }
        
        public void Initialize(List<IOrder> orders, List<RandomOrdersSettings> levelDataRandomOrdersSettings)
        {
            Cleanup();
            
            if (orders?.Count == 0 && levelDataRandomOrdersSettings == null)
            {
                levelDataRandomOrdersSettings = new List<RandomOrdersSettings> {_defaultRandomOrdersSettings};
            }

            if (orders?.Count == 0)
            {
                orders = GenerateOrders(levelDataRandomOrdersSettings).ToList();
            }

            foreach (var order in orders)
            {
                _ordersToComplete.Enqueue(order);
            }

            Debug.Log($"Initialized with {_ordersToComplete.Count} orders");
            _totalOrdersCount = _ordersToComplete.Count;
        }

        private IEnumerable<IOrder> GenerateOrders(List<RandomOrdersSettings> levelDataTotalOrdersCount)
        {
            foreach (var randomOrderSettings in levelDataTotalOrdersCount)
            {
                for(var i = 0; i < randomOrderSettings.OrdersCount; i++)
                {
                    // put a 1..3 unique worktypes from available types to the order
                    var workTypes = new List<WorkType>();
                    var workTypesCount = UnityEngine.Random.Range(1, 4);
                    var uniqueWorkTypes = randomOrderSettings.WorkTypes.ToList();
                    for (var j = 0; j < Math.Min(uniqueWorkTypes.Count, workTypesCount); j++)
                    {
                        var randomIndex = UnityEngine.Random.Range(0, uniqueWorkTypes.Count);
                        workTypes.Add(uniqueWorkTypes[randomIndex]);
                        uniqueWorkTypes.RemoveAt(randomIndex);
                    }
                    
                    var timeLimitSeconds = UnityEngine.Random.Range(randomOrderSettings.TimeLimitSecondsMin,
                        randomOrderSettings.TimeLimitSecondsMax);
                    yield return new Order(workTypes, timeLimitSeconds);
                }
            }
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

        public void Reset()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
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
            return _ordersToComplete.Count == 0 && _expiredOrders.Count + _completedOrders.Count == _totalOrdersCount;
        }

        private void CompleteLevel()
        {
            Debug.Log("Level completed, no more orders");
            LevelCompleted?.Invoke();
        }
    }
}