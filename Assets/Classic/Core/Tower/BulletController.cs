using System.Collections;
using Overwave.Classic.Utils;
using UnityEngine;

namespace Overwave.Classic.Tower
{
    public class BulletController : MonoBehaviour, IPoolable
    {
        [field: SerializeField]
        public BulletConfig Config { get; private set; }
        
        [field: SerializeField]
        public Behavior Tower { get; private set; }
        
        [field: SerializeField]
        public Enemy.Behavior Target { get; private set; }

        private int _currentIndex;
        
        public bool Deleted { get; set; }

        private IEnumerator Lifecycle()
        {
            yield return new WaitForSeconds(20);
            if (!Deleted) GameObjectPool.Delete(gameObject, Config.Id);
        }

        public void Init(Behavior tower, Enemy.Behavior target)
        {
            Tower = tower;
            Target = target;
            
            StartCoroutine(Lifecycle());
        }

        private void Update()
        {
            if (!Target)
            {
                Destroy();
                return;
            }

            GoToTarget();
        }

        private void GoToTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position,
                Config.Speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, Tower.transform.position) > (Tower.CurrentRange + 4) / 2)
                Destroy();
        }

        private void Destroy()
        {
            StopAllCoroutines();
            GameObjectPool.Delete(gameObject, Config.Id);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy") && other.TryGetComponent(out Enemy.Behavior enemy) &&
                !enemy.Deleted && enemy.RegisterDamageFromBullet(this))
            {
                Tower.Damage(enemy);
                Destroy();
            }
        }
    }
}