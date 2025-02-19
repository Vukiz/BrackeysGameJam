using System;
using System.Collections.Generic;
using Orders.Data;
using UnityEngine;

namespace Orders.Views
{
    public class OrderView : MonoBehaviour
    {
        [SerializeField] private List<WorkTypeToObjectPair> _workTypeToObjectPairs;
        
        public List<WorkTypeToObjectPair> WorkTypeToObjectPairs => _workTypeToObjectPairs;
    }
    
    [Serializable]
    public class WorkTypeToObjectPair
    {
        public WorkType WorkType;
        public GameObject Object;
    }
}