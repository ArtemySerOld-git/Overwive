using UnityEngine;

namespace Overwave.Classic.Core.Player
{
    [DisallowMultipleComponent, RequireComponent(typeof(Rigidbody))]
    public class Controller : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _drag = 3f;
        
        [Header("Keyboard")]
        [SerializeField] private KeyCode _forwardKey = KeyCode.W;
        [SerializeField] private KeyCode _backwayKey = KeyCode.S;
        [SerializeField] private KeyCode _leftKey = KeyCode.A;
        [SerializeField] private KeyCode _rightKey = KeyCode.D;
        
        [Header("Other")]
        [SerializeField] private Camera _camera;

        private Rigidbody _rb;
        private Vector3 _moveInput;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.linearDamping = _drag;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            
            if (!_camera)
                _camera = Camera.main;
        }

        private void Update()
        {
            _moveInput = Vector3.zero;
            
            if (Input.GetKey(_forwardKey)) _moveInput.z += 1;
            if (Input.GetKey(_backwayKey)) _moveInput.z -= 1;
            if (Input.GetKey(_leftKey))    _moveInput.x -= 1;
            if (Input.GetKey(_rightKey))   _moveInput.x += 1;
            
            _moveInput = _moveInput.normalized;
        }

        private void FixedUpdate()
        {
            if (_moveInput.magnitude < 0.1f) return;
            
            var cameraForward = _camera.transform.forward;
            var cameraRight = _camera.transform.right;
            
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();
            
            var moveDirection = cameraForward * _moveInput.z + cameraRight * _moveInput.x;
            
            var force = moveDirection * _moveSpeed;
            _rb.AddForce(force, ForceMode.Force);
        }
    }
}