using UnityEngine;

namespace Common
{
    public class LookAt2DComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 6f;
        [SerializeField] private Vector3 _worldUp;

        [Space]
        [Header("Runtime")]
        [SerializeField] private bool _isEnabled;


        public void SetIsEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        private void Update()
        {
            if (_isEnabled)
            {
                var lookRot = GetRotationTo(_target);
                
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, _speed * Time.deltaTime);
            }
        }
        
        Quaternion GetRotationTo(Transform target)
        {
            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
