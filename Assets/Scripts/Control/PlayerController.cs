namespace ParkingSimultaor.Control
{
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TiresController _tiresController;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _rotationSpeed = 10f;

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

            if (!Mathf.Approximately(_flatDirection.y, 0f))
            {
                var x = _flatDirection.x;
                
                if (_flatDirection.y < 0f)
                {
                    x *= -1f;
                }
                
                _rigidbody.rotation *= Quaternion.AngleAxis(x * _rotationSpeed * Time.deltaTime, Vector3.up);
            }

            var velocity = Vector3.forward * _flatDirection.y * _acceleration * Time.deltaTime;

            _rigidbody.AddRelativeForce(velocity, ForceMode.VelocityChange);

            var totalVelocityFraction = _rigidbody.velocity.magnitude / _maxSpeed;

            if (totalVelocityFraction > 1f)
            {
                _rigidbody.AddRelativeForce(-velocity, ForceMode.VelocityChange);
            }
        }

        private void OnDrawGizmos()
        {
            if (_rigidbody == null) return;

            Debug.DrawRay(_rigidbody.position, _rigidbody.velocity.normalized * 10f, Color.blue);
        }
    }
}