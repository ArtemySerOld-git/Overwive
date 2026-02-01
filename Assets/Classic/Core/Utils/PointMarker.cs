using System;
using UnityEditor;
using UnityEngine;

namespace Overwave.Classic.Utils
{
    [ExecuteInEditMode]
    public class PointMarker : MonoBehaviour
    {
        [SerializeField] private Color _lineColor = Color.green;
        [Header("Sphere"), SerializeField] private bool _showSphere = true;
        [SerializeField] private float _sphereSize = 0.1f;
        [SerializeField] private Color _sphereColor = Color.green;
        [Header("Points"), SerializeField] private bool _autoPoints = true;
        public GameObject[] points;
        
        public static PointMarker Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            if (Instance != this)
                Destroy(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _lineColor;
            
            if (points == null || points.Length <= 0 && _autoPoints)
                points = GameObject.FindGameObjectsWithTag("Point");

            for (var i = 0; i < points.Length - 1; i++)
                Gizmos.DrawLine(points[i].transform.position, points[i + 1].transform.position);
            
            #if UNITY_EDITOR
            if (points.Length > 0)
            {
                Handles.Label(points[0].transform.position,
                    new GUIContent(Resources.Load<Texture>("StartPoint")));
                Handles.Label(points[^1].transform.position,
                    new GUIContent(Resources.Load<Texture>("EndPoint")));
            }
            #endif

            if (_showSphere)
            {
                Gizmos.color = _sphereColor;
                for (var i = 1; i < points.Length - 1; i++)
                    Gizmos.DrawSphere(points[i].transform.position, _sphereSize);
            }
        }
    }
}