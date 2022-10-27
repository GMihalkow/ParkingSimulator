namespace ParkingSimultaor.Control
{
    using UnityEngine;

    public class TiresController : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] _tires;
        [SerializeField] private float _rotationSpeed = 5f;

        private bool _isActivated;
        private float _angle;
        public bool isActivated { set => _isActivated = value; }

        private void FixedUpdate()
        {
            if (!_isActivated) return;

            if (_angle >= 360f)
            {
                _angle = 0f;
            }
          
            foreach (var tire in _tires)
            {
                var rotation = Quaternion.AngleAxis(_angle, Vector3.right);
                tire.rotation = rotation;
            }

            _angle = Mathf.Clamp(_angle + Time.deltaTime * _rotationSpeed, 0f, 360f);
        }
    }
}