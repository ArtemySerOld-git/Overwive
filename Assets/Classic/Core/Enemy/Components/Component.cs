using Overwave.Classic.Tower;
using UnityEngine;

namespace Overwave.Classic.Enemy
{
    [System.Serializable]
    public abstract class Component
    {
        public abstract ComponentType Type { get; }
        
        [HideInInspector]
        public bool active;
        
        public virtual void Start() { }
        
        public virtual void Update() { }

        public virtual bool CanAddToTower(Tower.Behavior tower) => true;
        public virtual bool RegisterDamageFromBullet(BulletController bullet) => true;
    }
}