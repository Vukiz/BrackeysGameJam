using Orders.Infrastructure;
using SushiBelt.Views;

namespace SushiBelt.Infrastructure
{
    public interface ISushiBelt
    {
        void SetView(SushiBeltView sushiBeltView);
        IOrder CurrentOrder { get; }
        void SubmitOrder(IOrder order); // Order Provider works with this
        event System.Action<IOrder> OrderReceived; // Workstation works this
        event System.Action<IOrder> OrderCompleted; // Provider tracks this
        event System.Action<IOrder> OrderExpired; // Provider tracks this
        void Cleanup();
    }
}