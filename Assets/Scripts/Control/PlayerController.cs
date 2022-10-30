namespace ParkingSimultaor.Control
{
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TiresController _tiresController;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _breakForce = 5f;
        [SerializeField] private float _speedBreaksThreshold = 0.3f;

        private Rigidbody _rigidbody;
        private Vector3 _flatDirection;

        private void Awake()
        {
            _tiresController.isActivated = true;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _flatDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            var forwardForce = Vector3.forward * _flatDirection.y * _acceleration * Time.deltaTime;
            var localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);

            _rigidbody.AddRelativeForce(forwardForce, ForceMode.VelocityChange);

            _rigidbody.AddRelativeForce(Vector3.right * -localVelocity.x * Mathf.Abs(_flatDirection.x), ForceMode.VelocityChange);

            var localVelocityNormalized = localVelocity.normalized;
            var totalVelocityFraction = _rigidbody.velocity.magnitude / _maxSpeed;

            if (totalVelocityFraction > 1f)
            {
                _rigidbody.AddRelativeForce(-forwardForce, ForceMode.VelocityChange);
            }

            if (_rigidbody.velocity.magnitude < _speedBreaksThreshold) return;

            _rigidbody.AddRelativeForce(-localVelocityNormalized * Mathf.Clamp01(1f - Mathf.Abs(_flatDirection.y)) * _breakForce * Time.deltaTime, ForceMode.VelocityChange);
            _rigidbody.rotation *= Quaternion.AngleAxis(_flatDirection.x * _rotationSpeed * Time.deltaTime * localVelocityNormalized.z, Vector3.up);
        }

        private void OnDrawGizmos()
        {
            if (_rigidbody == null) return;

            Debug.DrawRay(_rigidbody.position, _rigidbody.velocity.normalized * 10f, Color.blue);
        }
    }
}