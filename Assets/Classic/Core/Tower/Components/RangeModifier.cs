using UnityEngine;

namespace Overwave.Classic.Tower.Components
{
    [System.Serializable]
    public class RangeModifier : Component
    {
        public override ComponentType Type => ComponentType.RangeModifier;
        
        [field: SerializeField]
        public float Range { get; private set; }

        private GameObject _rangeObject;
        
        private float _currentRange;
        public float CurrentRange
        {
            get => _currentRange;
            set
            {
                _currentRange = value;
                _rangeObject.transform.localScale = Vector3.one * Range;
            }
        }

        public override void Initialize()
        {
            _rangeObject = Tower.transform.Find("Range").gameObject;
            CurrentRange = Range;
        }
    }
}