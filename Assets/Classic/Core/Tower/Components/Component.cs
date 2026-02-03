using UnityEngine;

namespace Overwave.Classic.Tower 
{
    [System.Serializable]
    public abstract class Component
    {
        public Behavior Tower { get; set; }
        
        public abstract ComponentType Type { get; }
        
        [HideInInspector]
        public bool active = true;
        
        public virtual void Initialize() { }
        
        public virtual void Start() { }

        public virtual void Update() { }
        
        public virtual void DamageEnemy(Enemy.Behavior target) { }
        
        public virtual void OnFire() { }
    }
}
