using System;
using Rails.Views;

namespace Rails.Infrastructure
{
    public interface IRailSwitch : IWaypoint
    {
        void SetView(RailSwitchView view);
        event Action Rotated;
        void SetInteractable(bool isInteractable);
        void SetOutlineEnabled(bool isEnabled);
    }
}