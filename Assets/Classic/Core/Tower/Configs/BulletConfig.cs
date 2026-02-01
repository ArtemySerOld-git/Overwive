using UnityEngine;

namespace Overwave.Classic.Tower
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Tower/Bullet Config", order = -1000)]
    public class BulletConfig : BaseConfig
    {
        [field: Space, SerializeField]
        public GameObject Prefab { get; private set; }
        
        [field: SerializeField]
        public float Speed { get; private set; }

        public override Object Value => Prefab;
    }
}