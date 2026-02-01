using UnityEngine;

namespace Overwave.Classic.Tower.Components
{
    [System.Serializable]
    public class SeeFly : Component
    {
        [field: SerializeField]
        public bool Value { get; private set; }

        public override ComponentType Type => ComponentType.SeeFly;
    }
}