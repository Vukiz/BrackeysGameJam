using Orders;

namespace SushiBelt
{
    public interface ISushiBelt
    {
        IOrder CurrentOrder { get; }
        System.Action<IOrder> OrderReceived { get; set; } // Workstation works this
        System.Action<IOrder> OrderCompleted { get; set; } // Provider tracks this
    }
}