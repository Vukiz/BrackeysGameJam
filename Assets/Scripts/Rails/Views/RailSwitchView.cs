using System;
using System.Collections.Generic;
using Level.Infrastructure;
using MoreMountains.Feedbacks;
using Rails.Implementation;
using UnityEngine;

namespace Rails.Views
{
    public class RailSwitchView : WaypointView, IInteractable
    {
        [SerializeField] private List<WaypointView> _neighbourWaypoints;
        [SerializeField] private MMF_Player _player;

        public List<WaypointView> NeighbourWaypoints => _neighbourWaypoints;

        public event Action InteractionRequired;

        public void Interact()
        {
            _player.PlayFeedbacks();

            InteractionRequired?.Invoke();
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