using Rails.Infrastructure;
using UnityEngine;

namespace Rails.Implementation
{
    public class IntermediateWaypoint : MonoBehaviour, IIntermediateWaypoint
    {
        public Vector3 Position => transform.position;
    }
}