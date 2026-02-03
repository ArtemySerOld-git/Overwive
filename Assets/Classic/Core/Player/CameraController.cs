using UnityEngine;

namespace Overwave.Classic.Core.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _distance = 5f;
        
        [Header("Speed")]
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private float _xSpeed;
        [SerializeField] private float _ySpeed;
        
        [Header("Min/Max")]
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _yMinLimit;
        [SerializeField] private float _yMaxLimit;
        
        [field: SerializeField, Space] public Transform Target { get; private set; }

        private float _x;
        private float _y;
        private float _targetDistance;

        private void LateUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                _x += Input.GetAxis("Mouse X") * _xSpeed * Time.deltaTime;
                _y -= Input.GetAxis("Mouse Y") * _ySpeed * Time.deltaTime;
                
                _y = Mathf.Clamp(_y, _yMinLimit, _yMaxLimit);
            }
            
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                _targetDistance -= scroll * _scrollSpeed;
                _targetDistance = Mathf.Clamp(_targetDistance, _minDistance, _maxDistance);
            }
            
            _distance = Mathf.Lerp(_distance, _targetDistance, Time.deltaTime * 5f);
            var rotation = Quaternion.Euler(_y, _x, 0);
            var pos = rotation * new Vector3(0f, 0f, -_distance) + Target.position;
            
            transform.SetPositionAndRotation(pos, rotation);
        }
    }
}