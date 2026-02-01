using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Overwave.Classic.Enemy
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Enemy Config", order = -1000)]
    public class Config : BaseConfig
    {
        [field: SerializeField]
        public GameObject Prefab { get; private set; }

        [field: Header("Base Stats"), SerializeField]
        public int Health { get; private set; }
        [field: SerializeField]
        public int Speed { get; private set; }
        [field: SerializeField]
        public float RotationSpeed { get; private set; }
        
        [field: Header("Other"), SerializeField]
        public byte AnimationsCount { get; private set; }
        
        [field: Space, SerializeReference]
        public List<Component> Components { get; private set; }
        
        public override Object Value => Prefab;

        public bool HasComponent(ComponentType type)
            => Components.Exists(component => component.Type == type);
        
        public Component TryGetComponent(ComponentType type)
            => Components.FirstOrDefault(component => component.Type == type);
    }
}