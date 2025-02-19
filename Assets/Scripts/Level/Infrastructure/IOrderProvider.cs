using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Level.Data;
using Orders;
using SushiBelt.Infrastructure;

namespace Level.Infrastructure
{
    public interface IOrderProvider
    {
        event System.Action LevelCompleted;
        void Initialize(List<IOrder> orders, List<RandomOrdersSettings> levelDataRandomOrdersSettings);
        void RegisterSushiBelt(ISushiBelt sushiBelt);
        
        UniTaskVoid StartProcessingOrders();
        
        int  GetFailedOrdersCount();
        int  GetCompletedOrdersCount();
        bool IsLevelWon();
    }
}