using Level.Views;

namespace Level.Infrastructure
{
    public interface ILevelConfigurator
    {
        LevelView SpawnLevel(int number);
    }
}