using Cysharp.Threading.Tasks;

namespace StateMachine.Views
{
    public class LoaderView : BaseView
    {
        public override UniTask Show()
        {
            base.Show();
            return UniTask.CompletedTask;
        }
        
        public override UniTask Hide()
        {
            base.Hide();
            return UniTask.CompletedTask;
        }
    }
}