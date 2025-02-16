namespace Level
{
    public interface IOrderProvider
    {
        System.Action LevelCompleted { get; set; } // Track this to show the end screen and start the next level
    }
}