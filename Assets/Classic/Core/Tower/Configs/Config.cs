using System.Collections.Generic;
using UnityEngine;

namespace Overwave.Classic.Tower
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Tower/Config", order = -1000)]
    public class Config : BaseConfig
    {
        [field: SerializeField]
        public PreviewConfig Preview { get; private set; }
        
        [field: SerializeField]
        public LayerMask AllowedPlatforms { get; private set; }
        
        [field: Space, Range(0, 40), SerializeField]
        public int MaxPlacementCount { get; private set; }
        
        [field: SerializeField]
        public BulletConfig Bullet { get; private set; }
        
        [field: SerializeField]
        public LevelConfig[] Levels { get; private set; }
        
        public override Object Value => Preview;
    }
}