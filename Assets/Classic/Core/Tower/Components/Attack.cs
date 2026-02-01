using UnityEngine;

namespace Overwave.Classic.Tower.Components
{
    [System.Serializable]
    public class Attack : Component
    {
        [field: SerializeField]
        public float Damage { get; private set; }
        [field: SerializeField]
        public float FireRate { get; private set; }
        
        private float _currentFireRate;

        public override ComponentType Type => ComponentType.Attack;

        public override void Initialize()
        {
            _currentFireRate = FireRate;
        }

        public override void Update()
        {
            if (Tower.enemies.Count == 0) return;
            
            _currentFireRate -= Time.deltaTime;
            if (_currentFireRate <= 0)
            {
                _currentFireRate = FireRate;
                Tower.Fire(Tower.GetPriorityTarget());
            }
        }
        
        public override void DamageEnemy(Enemy.Behavior target)
        {
            target.CurrentHealth -= Damage;
        }
    }
}