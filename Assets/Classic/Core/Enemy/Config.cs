using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]

namespace Overwave.Classic.Enemy
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Enemy Config", order = -1000)]
    public class Config : BaseConfig
    {
        [field: Space, SerializeField]
        public GameObject Prefab { get; internal set; }

        [field: Header("Movement"), SerializeField] public float Speed { get; internal set; } = 1;
        [field: SerializeField] public float RotationSpeed { get; internal set; } = 1.5f;
        
        [field: Header("Life"), SerializeField] public int Health { get; internal set; } = 10;
        [field: SerializeReference] public List<Component> Components { get; internal set; }
        
        [field: Header("Other"), SerializeField] public int AnimationsCount { get; internal set; } = 3;
        [field: SerializeField] public ClassType ClassType { get; internal set; } = ClassType.Standard;
        
        public override Object Value => Prefab;

        public bool HasComponent(ComponentType type)
            => Components.Exists(component => component.Type == type);
        
        public Component TryGetComponent(ComponentType type)
            => Components.FirstOrDefault(component => component.Type == type);
    }
}