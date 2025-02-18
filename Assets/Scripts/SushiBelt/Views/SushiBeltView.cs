using UnityEngine;

namespace SushiBelt.Views
{
    public class SushiBeltView : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private Transform _targetPoint;
        
        public Transform StartPoint => _startPoint;
        public Transform EndPoint => _endPoint;
        public Transform TargetPoint => _targetPoint;
    }
}