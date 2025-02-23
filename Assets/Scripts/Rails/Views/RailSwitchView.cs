using System;
using System.Collections.Generic;
using Level.Infrastructure;
using MoreMountains.Feedbacks;
using Rails.Implementation;
using UnityEngine;
using VFX.Implementation;

namespace Rails.Views
{
    public class RailSwitchView : WaypointView, IInteractable
    {
        [SerializeField] private List<WaypointView> _neighbourWaypoints;
        [SerializeField] private MMF_Player _player;
        [SerializeField] private Outliner _outliner;

        public List<WaypointView> NeighbourWaypoints => _neighbourWaypoints;

        public event Action InteractionRequired;

        public void Interact()
        {
            _player.PlayFeedbacks();

            InteractionRequired?.Invoke();
        }

        public void AnimateOutline()
        {
            _outliner.AnimateOutline(1f, 0.5f, false, false);
        }

        public void DisableOutline()
        {
            _outliner.DisableOutline();
        }

        private void OnDrawGizmos()
        {
            foreach (var neighbourWaypoint in NeighbourWaypoints)
            {
                if (neighbourWaypoint == null)
                {
                    continue;
                }

                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, neighbourWaypoint.transform.position);
            }
        }
    }
}