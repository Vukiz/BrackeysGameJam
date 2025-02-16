namespace Workstation
{
    public interface ISlot
    {
        bool IsOccupied { get; }
        
        event System.Action Occupied;
        
        void Occupy();
    }
}