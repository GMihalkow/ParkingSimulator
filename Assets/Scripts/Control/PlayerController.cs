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
            _flatDirection = new Vector3(1f, 0f, 1f);
            _tiresController.isActivated = true;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var horizontalAxis = Input.GetAxis("Horizontal");
            var verticalAxis = Input.GetAxis("Vertical");

            _rigidbody.AddRelativeForce(Vector3.forward * verticalAxis * _acceleration * Time.deltaTime, ForceMode.VelocityChange);

            //_rigidbody.velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);

            //Debug.Log(_rigidbody.velocity.magnitude);

            if (Mathf.Approximately(verticalAxis, 0f)) return;

            //var dir = Quaternion.LookRotation(velocity, Vector3.up);
            //_rigidbody.MoveRotation(Quaternion.AngleAxis(horizontalAxis, Vector3.up));
            _rigidbody.rotation *= Quaternion.AngleAxis(horizontalAxis * _rotationSpeed * Time.deltaTime, Vector3.up);
        }
    }
}