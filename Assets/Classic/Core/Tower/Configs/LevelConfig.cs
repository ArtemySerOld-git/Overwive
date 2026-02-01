using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Overwave.Classic.Tower
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Tower/Level Config", order = -1000)]
    public class LevelConfig : BaseConfig
    {
        [field: SerializeReference]
        public List<Component> Components { get; private set; }
            
        [field: SerializeField]
        public GameObject Prefab { get; private set; }
            
        [field: SerializeField]
        public int Cost { get; private set; }

        public override Object Value => Prefab;

        public bool HasComponent(ComponentType type)
            => Components.Exists(component => component.Type == type);

        public Component TryGetComponent(ComponentType type)
            => Components.FirstOrDefault(component => component.Type == type);
    }
}