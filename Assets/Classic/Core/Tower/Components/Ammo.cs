using UnityEngine;

namespace Overwave.Classic.Tower.Components
{
    [System.Serializable]
    public class Ammo : Component
    {
        [field: SerializeField]
        public int MaxAmmo { get; private set; }
        
        [field: SerializeField]
        public float ReloadTime { get; private set; }
        
        private int _currentAmmo;
        private float _currentReloadTime;
        private Component _attackComponent;

        public override ComponentType Type => ComponentType.Ammo;

        public override void Initialize()
        {
            _currentAmmo = MaxAmmo;
            _currentReloadTime = ReloadTime;
            _attackComponent = Tower.Config.TryGetComponent(ComponentType.Attack);
        }

        public override void Update()
        {
            if (_currentReloadTime > 0)
            {
                _currentReloadTime -= Time.deltaTime;
                _attackComponent.active = false;
            }
            else if (_attackComponent is { active: false }) _attackComponent.active = true;
        }

        public override void OnFire()
        {
            _currentAmmo--;
            Debug.Log($"Ammo: {_currentAmmo} / {MaxAmmo}");
            if (_currentAmmo <= 0)
            {
                _currentAmmo = MaxAmmo;
                _currentReloadTime = ReloadTime;
            }
        }
    }
}