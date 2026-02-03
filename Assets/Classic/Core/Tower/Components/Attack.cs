using UnityEngine;

namespace Overwave.Classic.Tower.Components
{
    [System.Serializable]
    public class Attack : Component
    {
        private static readonly int IsFire = Animator.StringToHash("IsFire");
        private static readonly int Rate = Animator.StringToHash("FireRate");

        [field: SerializeField]
        public float Damage { get; private set; }
        [field: SerializeField]
        public float FireRate { get; private set; }
        
        private float _currentFireRate;
        public float CurrentFireRate
        {
            get => _currentFireRate;
            set
            {
                _currentFireRate = value;
                Tower.Animator.SetFloat(Rate, _currentFireRate);
            }
        }

        public override ComponentType Type => ComponentType.Attack;

        public override void Initialize()
        {
            CurrentFireRate = FireRate;
        }

        public override void Update()
        {
            if (Tower.enemies.Count == 0) return;
            Tower.Animator.SetBool(IsFire, true);
            
            CurrentFireRate -= Time.deltaTime;
            if (_currentFireRate <= 0)
            {
                CurrentFireRate = FireRate;
                Tower.Fire(Tower.GetPriorityTarget());
            }
        }
        
        public override void DamageEnemy(Enemy.Behavior target)
        {
            target.CurrentHealth -= Damage;
        }
    }
}