using Cysharp.Threading.Tasks;
using Tutorial.Views;

namespace Tutorial.Infrastructure
{
    public interface IDemonstrationSequence
    {
        UniTask Demonstrate(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView);
    }
}