using System.Collections;
using Overwave.Classic.Utils;
using UnityEngine;

namespace Overwave.Classic.GameMode
{
    public class EnemyWaveManager : MonoBehaviour
    {
        private Vector3 _summonPos;

        public EnemyWaveConfig config;
        public System.Action End = delegate { };

        IEnumerator Spawn()
        {
            yield return new WaitForSeconds(config.StartDelay);
            
            for (var i = 0; i < config.Count; i++)
            {
                GameObjectPool.Summon(config.Config.Id, _summonPos);
                yield return new WaitForSeconds(config.Interval);
            }

            Debug.Log($"Ended '{config.name}' MiniWave Manager");
            End();
            End = delegate { };
            Destroy(gameObject);
        }

        public void Init(EnemyWaveConfig cfg)
        {
            config = cfg;

            _summonPos = MainManager.Instance.startPortal.transform.position;
            StartCoroutine(Spawn());
        }
    }
}