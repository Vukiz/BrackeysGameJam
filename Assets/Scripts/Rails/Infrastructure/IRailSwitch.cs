using Rails.Views;

namespace Rails.Infrastructure
{
    public interface IRailSwitch : IWaypoint
    {
        void SetView(RailSwitchView view);
    }
}