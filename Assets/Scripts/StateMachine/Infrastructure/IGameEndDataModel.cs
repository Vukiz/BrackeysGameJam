using StateMachine.Data;

namespace StateMachine.Infrastructure
{
    public interface IGameEndDataModel
    {
        GameEndType GameEndType { get; set; }
    }
}