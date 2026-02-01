using UnityEngine;

namespace Overwave.Classic.Tower
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Tower/Preview Config", order = -1000)]
    public class PreviewConfig : BaseConfig
    {
        [field: Space, SerializeField]
        public GameObject Model { get; private set; }
        [field: SerializeField]
        public Vector3 PlacementOffset { get; private set; }

        public override Object Value => Model;
    }
}