using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Orders;
using SushiBelt.Infrastructure;

namespace Level.Infrastructure
{
    public interface IOrderProvider
    {
        event System.Action LevelCompleted;
        void Initialize(IEnumerable<IOrder> orders);
        void RegisterSushiBelt(ISushiBelt sushiBelt);
        
        UniTaskVoid StartProcessingOrders();
        
        int  GetFailedOrdersCount();
        int  GetCompletedOrdersCount();
        bool IsLevelWon();
    }
}