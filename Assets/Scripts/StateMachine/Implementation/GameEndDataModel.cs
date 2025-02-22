using StateMachine.Data;
using StateMachine.Infrastructure;

namespace StateMachine.Implementation
{
    public class GameEndDataModel : IGameEndDataModel
    {
        public GameEndType GameEndType { get; set; }
    }
}