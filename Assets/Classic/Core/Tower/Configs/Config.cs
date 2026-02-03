using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]

namespace Overwave.Classic.Tower
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Tower/Config", order = -1000)]
    public class Config : BaseConfig
    {
        [field: SerializeField]
        public PreviewConfig Preview { get; internal set; }
        
        [field: SerializeField]
        public LayerMask AllowedPlatforms { get; internal set; }

        [field: Space, Range(0, 40), SerializeField]
        public int MaxPlacementCount { get; internal set; } = 40;
        
        [field: SerializeField]
        public BulletConfig Bullet { get; internal set; }

        [field: SerializeField] public List<LevelConfig> Levels { get; internal set; } = new();

        [field: SerializeField] public ClassType ClassType { get; internal set; } = ClassType.Starter;
        
        public override Object Value => Preview;
    }
}