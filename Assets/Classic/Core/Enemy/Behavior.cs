using System;
using System.Linq;
using Overwave.Classic.Tower;
using Overwave.Classic.Utils;
using UnityEngine;

namespace Overwave.Classic.Enemy
{
    [DisallowMultipleComponent, RequireComponent(typeof(Animator)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(BoxCollider))]
    public class Behavior : MonoBehaviour, IPoolable
    {
        private static readonly int SpeedMultiplier = Animator.StringToHash("SpeedMultiplier");
        private int _currentPointIndex;
        private float _totalPathLength;
        private Animator _animator;
        
        [field: SerializeField]
        public Config Config { get; private set; }

        [SerializeField] private float _currentHealth;
        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                if (_currentHealth <= 0)
                    GameObjectPool.Delete(gameObject, Config.Id);
            }
        }

        [SerializeField] private float _currentSpeed;
        public float CurrentSpeed
        {
            get => _currentSpeed;
            set
            {
                _currentSpeed = value;
                _animator.SetFloat(SpeedMultiplier, value);
            }
        }

        [field: SerializeField]
        public float PathProgress { get; private set; }
        
        public GameObject[] Points { get; private set; }
        
        public bool Deleted { get; set; }

        public bool CanAddToTower(Tower.Behavior tower)
            => AllIfActive(c => c.CanAddToTower(tower));
        
        public bool RegisterDamageFromBullet(BulletController bullet)
            => AllIfActive(c => c.RegisterDamageFromBullet(bullet));

        public void OnSummon()
        {
            Points ??= PointMarker.Instance.points;
            
            _currentPointIndex = FindCurrentSegmentIndex() + 1;

            CurrentHealth = Config.Health;
            
            if (_currentPointIndex >= Points.Length)
                _currentPointIndex = Points.Length - 1;

            InitializePathProgress();
            
            CallIfActive(c => c.Start());
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            CurrentSpeed = Config.Speed;
        }

        private void Update()
        {
            Move();
            UpdateProgress();

            CallIfActive(c => c.Update());
        }

        private void Move()
        {
            if (Points == null || Points.Length < 2 
                               || _currentPointIndex >= Points.Length || Points[_currentPointIndex] == null)
                return;

            var targetPos = Points[_currentPointIndex].transform.position;

            var direction = (targetPos - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Config.RotationSpeed * Time.deltaTime);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, Config.Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                _currentPointIndex++;
        }
        
        private int FindCurrentSegmentIndex()
        {
            var closestSegment = 0;
            var minDistance = float.MaxValue;

            for (var i = 0; i < Points.Length - 1; i++)
            {
                var a = Points[i].transform.position;
                var b = Points[i + 1].transform.position;

                var dist = DistanceToSegment(transform.position, a, b);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestSegment = i;
                }
            }
            return closestSegment;
        }

        private static float DistanceToSegment(Vector3 point, Vector3 a, Vector3 b)
        {
            var ab = b - a;
            var ap = point - a;

            var projection = Vector3.Dot(ap, ab.normalized);
            if (projection <= 0) return Vector3.Distance(point, a);
            if (projection >= ab.magnitude) return Vector3.Distance(point, b);

            return Vector3.Cross(ab, ap).magnitude / ab.magnitude;
        }

        private void InitializePathProgress()
        {
            _totalPathLength = 0f;

            for (var i = _currentPointIndex - 1; i < Points.Length - 1; i++)
            {
                _totalPathLength += Vector3.Distance(
                    Points[i].transform.position,
                    Points[i + 1].transform.position
                );
            }
        }

        private void UpdateProgress()
        {
            if (_currentPointIndex == 0) return;

            var traveledToCurrent = 0f;
            for (var i = 0; i < _currentPointIndex - 1; i++)
                traveledToCurrent += Vector3.Distance(
                    Points[i].transform.position, 
                    Points[i + 1].transform.position
                );

            var currentSegmentDistance = Vector3.Distance(Points[_currentPointIndex - 1].transform.position, transform.position);

            PathProgress = (traveledToCurrent + currentSegmentDistance) / _totalPathLength; 
        }

        private void CallIfActive(Action<Component> action)
        {
            Config.Components.ForEach(component => { if (component.active) action(component); });
        }
        
        private bool AllIfActive(Func<Component, bool> action)
            => Config.Components.All(component => component.active && action(component));
    }
}