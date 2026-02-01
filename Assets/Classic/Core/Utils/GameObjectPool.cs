using System.Collections.Generic;
using Overwave.Classic.GameMode;
using Overwave.Classic.Tower;
using UnityEngine;

namespace Overwave.Classic.Utils
{
    public static class GameObjectPool
    {
        private class PoolEntry
        {
            public readonly Queue<GameObject> Inactive = new();
            public readonly HashSet<GameObject> Active = new();
        }
        
        private static readonly Dictionary<string, PoolEntry> _pools = new();

        public static void LoadGameMode(GameModeConfig config)
        {
            Dictionary<Enemy.Config, int> counts = new();

            foreach (var wave in config.Structure)
            {
                foreach (var miniWave in wave.Structure)
                {
                    if (!counts.TryAdd(miniWave.Config, miniWave.Count))
                        counts[miniWave.Config] += miniWave.Count;
                }
            }

            foreach (var pair in counts)
            {
                if (!_pools.ContainsKey(pair.Key.Id))
                    _pools[pair.Key.Id] = new PoolEntry();
                
                var entry = _pools[pair.Key.Id];
                for (var i = 0; i < pair.Value; i++)
                {
                    var go = Assets.Manager.GetAny<GameObject>(pair.Key.Id);
                    if (go == null)
                    {
                        Debug.LogError($"ID '{pair.Key.Id}' not found");
                        continue;
                    }
                    
                    var obj = Object.Instantiate(go);
                    obj.SetActive(false);
                    entry.Inactive.Enqueue(obj);
                }
                Debug.Log($"Loaded {pair.Value} {pair.Key.Id}");
            }
        }

        public static void LoadGameMode(string id)
        {
            var config = Assets.Manager.GetAny<GameModeConfig>(id);
            if (config) LoadGameMode(config);
        }

        public static void LoadTower(Config config)
        {
            for (var i = 0; i < config.MaxPlacementCount / 2; i++)
            {
                foreach (var level in config.Levels)
                {
                    if (!_pools.ContainsKey(level.Id))
                        _pools[level.Id] = new PoolEntry();
                    var tower = Object.Instantiate(level.Prefab);
                    tower.SetActive(false);

                    var pool = _pools[level.Id];
                    pool.Inactive.Enqueue(tower);
                }
            }

            var preview = Object.Instantiate(config.Preview.Model);
            preview.SetActive(false);
            
            if (!_pools.ContainsKey(config.Preview.Id))
                _pools[config.Preview.Id] = new PoolEntry();
            var entry = _pools[config.Preview.Id];
            entry.Inactive.Enqueue(preview);

            if (!_pools.ContainsKey(config.Bullet.Id))
                _pools[config.Bullet.Id] = new PoolEntry();
            var bulletEntry = _pools[config.Bullet.Id];

            for (var i = 0; i < 10; i++)
            {
                var bullet = Object.Instantiate(config.Bullet.Prefab);
                bullet.SetActive(false);
                
                bulletEntry.Inactive.Enqueue(bullet);
            }
        }

        public static void LoadTower(string id)
        {
            var config = Assets.Manager.Get<Config>(id);
            if (config) LoadTower(config);
        }
        
        public static void LoadTowers(string[] ids)
        {
            foreach (var id in ids)
                LoadTower(id);
        }

        public static GameObject Summon(string id, Vector3 position, Quaternion rotation)
        {
            if (!_pools.TryGetValue(id, out var pool)) return null;
            
            var obj = pool.Inactive.Count > 0 ? pool.Inactive.Dequeue()
                : Object.Instantiate(Assets.Manager.GetAny<GameObject>(id));
            
            obj.SetActive(true);
            obj.transform.SetPositionAndRotation(position, rotation);
            pool.Active.Add(obj);

            if (obj.TryGetComponent(out IPoolable poolable))
            {
                poolable.OnSummon();
                poolable.Deleted = false;
            }

            return obj;
        }
        public static GameObject Summon(string id, Vector3 position)
            => Summon(id, position, Quaternion.identity);
        public static GameObject Summon(string id, Transform transform)
            => Summon(id, transform.position, transform.rotation);

        public static void Delete(GameObject obj, string id)
        {
            if (!_pools.TryGetValue(id, out var pool)) return;

            if (pool.Active.Remove(obj))
            {
                obj.SetActive(false);
                pool.Inactive.Enqueue(obj);

                if (obj.TryGetComponent(out IPoolable poolable))
                {
                    poolable.OnDelete();
                    poolable.Deleted = true;
                }
            }
        }
    }
}