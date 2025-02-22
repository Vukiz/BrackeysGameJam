namespace StateMachine.Infrastructure
{
    public interface IGameLevelDataModel
    {
        public bool IsTutorialFinished { get; set; }
        public int CurrentLevelIndex { get; set; }
    }
}