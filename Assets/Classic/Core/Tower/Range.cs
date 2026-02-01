using UnityEngine;

namespace Overwave.Classic.Tower
{
    [DisallowMultipleComponent, RequireComponent(typeof(SphereCollider))]
    public class Range : MonoBehaviour
    {
        private Behavior _base;

        private void Start()
        {
            _base = GetComponentInParent<Behavior>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") && other.TryGetComponent(out Enemy.Behavior behavior) &&
                !behavior.Deleted && behavior.CanAddToTower(_base) && !_base.enemies.Contains(behavior))
                _base.enemies.Add(behavior);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy") && other.TryGetComponent(out Enemy.Behavior behavior) &&
                !behavior.Deleted && _base.enemies.Contains(behavior))
                _base.enemies.Remove(behavior);
        }
    }
}