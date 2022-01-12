using System;
using UnityEngine;

namespace Prototype
{
    public class GizmoHighlighter : MonoBehaviour
    {
        [SerializeField] private Color _color = Color.magenta;
        [SerializeField] private float _size = 1f;
        [SerializeField] private int _shapeType = 1;

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            if (_shapeType == 0)
            {
                Gizmos.DrawCube(transform.position, Vector3.one * _size);
            }
            else
            {
                Gizmos.DrawSphere(transform.position, _size);
            }
        }
    }
}
