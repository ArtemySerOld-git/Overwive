using UnityEngine;

namespace Overwave.Classic.Tower.Components
{
    [System.Serializable]
    public class SeeHidden : Component
    {
        [field: SerializeField]
        public bool Value { get; private set; }

        public override ComponentType Type => ComponentType.SeeHidden;
    }
}