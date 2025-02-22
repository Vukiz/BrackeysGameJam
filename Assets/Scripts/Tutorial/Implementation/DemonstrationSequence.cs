using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tutorial.Infrastructure;
using Tutorial.Views;

namespace Tutorial.Implementation
{
    public class DemonstrationSequence : IDemonstrationSequence
    {
        private delegate UniTask Demonstration(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView);
        
        private readonly List<Demonstration> _demonstrations = new();
        
        public async UniTask Demonstrate(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            foreach (var demonstration in _demonstrations)
            {
                await demonstration(tutorialCanvasView, tutorialView);
            }
        }
    }
}