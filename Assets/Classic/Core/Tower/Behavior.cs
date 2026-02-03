using System;
using System.Collections.Generic;
using System.Linq;
using Overwave.Classic.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Overwave.Classic.Tower
{
    [DisallowMultipleComponent, RequireComponent(typeof(Animator)), RequireComponent(typeof(BoxCollider))]
    public class Behavior : MonoBehaviour, IPoolable
    {
        [field: SerializeField]
        public Config TowerConfig { get; set; }
        
        [field: SerializeField]
        public LevelConfig Config { get; set; }
        
        [field: SerializeField]
        public TargetPriority Priority { get; private set; }
        
        [field: SerializeField]
        public Animator Animator { get; private set; }
        
        public List<Enemy.Behavior> enemies = new();

        public bool Deleted { get; set; }
        
        public void Damage(Enemy.Behavior target)
            => CallIfActive(c => c.DamageEnemy(target));

        public void Fire(Enemy.Behavior target)
        {
            var bullet = GameObjectPool.Summon(TowerConfig.Bullet.Id, transform);
            bullet.GetComponent<BulletController>().Init(this, target);
            
            CallIfActive(c => c.OnFire());
        }

        private void Start()
        {
            Animator = GetComponent<Animator>();
            
            Config = Instantiate(Config);
            Config.Components.ForEach(comp => { comp.Tower = this; comp.Initialize(); });
        }

        public void OnSummon()
        {
            CallIfActive(c => c.Start());
        }

        public override string ToString() => Config.Id;

        private void Update()
        {
            CallIfActive(c => c.Update());
        }

        public Enemy.Behavior GetPriorityTarget()
        {
            return enemies[Priority switch
            {
                TargetPriority.First => GetFirstEnemy(),
                TargetPriority.Last => GetLastEnemy(),
                TargetPriority.Nearest => GetNearestEnemy(),
                TargetPriority.Father => GetFatherEnemy(),
                TargetPriority.Strongest => GetStrongestEnemy(),
                TargetPriority.Weak => GetWeakEnemy(),
                TargetPriority.Random => GetRandomEnemy(),
                _ => GetFirstEnemy()
            }];
        }

        private int GetFirstEnemy()
        {
            if (enemies == null || enemies.Count == 0) return -1;

            var maxIndex = -1;
            var maxProgress = float.MinValue;

            for (var i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i] || enemies[i].Deleted) continue;

                if (enemies[i].PathProgress > maxProgress)
                {
                    maxProgress = enemies[i].PathProgress;
                    maxIndex = i;
                }
            }
            return maxIndex;
        }
        private int GetLastEnemy()
        {
            if (enemies == null || enemies.Count == 0) return -1;

            var lastIndex = -1;
            var lastProgress = float.MaxValue;

            for (var i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i] || enemies[i].Deleted) continue;

                if (enemies[i].PathProgress < lastProgress)
                {
                    lastProgress = enemies[i].PathProgress;
                    lastIndex = i;
                }
            }
            return lastIndex;
        }
        private int GetNearestEnemy()
        {
            if (enemies == null || enemies.Count == 0) return -1;

            var nearestIndex = -1;
            var minDistance = float.MaxValue;

            for (var i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i] || enemies[i].Deleted) continue;

                var distance = Vector3.Distance(enemies[i].transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestIndex = i;
                }
            }
            return nearestIndex;
        }
        private int GetFatherEnemy()
        {
            if (enemies == null || enemies.Count == 0) return -1;

            var fatherIndex = -1;
            var maxDistance = float.MinValue;

            for (var i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i] || enemies[i].Deleted) continue;

                var distance = Vector3.Distance(enemies[i].transform.position, transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    fatherIndex = i;
                }
            }
            return fatherIndex;
        }
        private int GetStrongestEnemy()
        {
            if (enemies == null || enemies.Count == 0) return -1;

            var strongestIndex = -1;
            var highestHealth = float.MinValue;

            for (var i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i] || enemies[i].Deleted) continue;

                if (enemies[i].CurrentHealth > highestHealth)
                {
                    highestHealth = enemies[i].CurrentHealth;
                    strongestIndex = i;
                }
            }
            return strongestIndex;
        }
        private int GetWeakEnemy()
        {
            if (enemies == null || enemies.Count == 0) return -1;

            var weakIndex = -1;
            var lowestHealth = float.MaxValue;

            for (var i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i] || enemies[i].Deleted) continue;

                if (enemies[i].CurrentHealth < lowestHealth)
                {
                    lowestHealth = enemies[i].CurrentHealth;
                    weakIndex = i;
                }
            }
            return weakIndex;
        }
        private int GetRandomEnemy()
        {
            if (enemies == null || enemies.Count == 0) return -1;

            return Random.Range(0, enemies.Count);
        }

        private void CallIfActive(Action<Component> action)
            => Config.Components.ForEach(c => { if (c.active) action(c); });
        
        private bool AllIfActive(Func<Component, bool> action)
            => Config.Components.All(component => component.active && action(component));
    }
}