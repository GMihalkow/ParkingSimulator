namespace ParkingSimultaor.Control
{
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private Transform _target;
        [SerializeField] Vector3 _offset = Vector3.back * 5f + Vector3.up * 3f;

        private float _xAngle;
        private Transform _cachedTransform;

        private void Awake() => _cachedTransform = transform;

        private void Start() => Cursor.lockState = CursorLockMode.Locked;

        private void Update()
        {
            var xInput = Input.GetAxis("Mouse X");

            _xAngle += xInput * _rotationSpeed * Time.deltaTime;

            var yRot = Quaternion.AngleAxis(_xAngle, Vector3.up);

            _cachedTransform.position = _target.position + yRot * _offset;

            var direction = _target.position - _cachedTransform.position;
            _cachedTransform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}