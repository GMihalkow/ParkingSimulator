namespace ParkingSimultaor.Control
{
    using Dreamteck.Car;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _maxTurnAngle = 30f;
        [SerializeField] private float _maxTorque = 20f;
        [SerializeField] private WheelCollider _backLeftWheel;
        [SerializeField] private WheelCollider _backRightWheel;
        [SerializeField] private WheelCollider _frontLeftWheel;
        [SerializeField] private WheelCollider _frontRightWheel;
        [SerializeField] private Transform _backLeftTransform;
        [SerializeField] private Transform _backRightTransform;
        [SerializeField] private Transform _frontLeftTransform;
        [SerializeField] private Transform _frontRightTransform;
        [SerializeField] private float _brakeDuration;
        [SerializeField] private float _brakeMaxForce = 50f;
        [SerializeField] private float _fallOverResetTimeout = 2f;
        [SerializeField] private WheelsScanner _wheelsScanner;

        private float _brakeStartTime;
        private Vector2 _input;
        private float _fellOverAt = -1;

        private void Update()
        {
            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Input.GetKeyDown(KeyCode.E) && _wheelsScanner.IsScanSuccessful())
            {
                Debug.Log("PARKING SUCCESSFUL!");
            }

            HandleBrakes();
            ValidatePosition();
        }

        private void FixedUpdate()
        {
            var turnAngle = _maxTurnAngle * _input.x;

            _frontLeftWheel.steerAngle = turnAngle;
            _frontRightWheel.steerAngle = turnAngle;

            _frontLeftWheel.motorTorque = _input.y * _maxTorque;
            _frontRightWheel.motorTorque = _input.y * _maxTorque;

            UpdateWheelPosition(_backLeftWheel, _backLeftTransform);
            UpdateWheelPosition(_frontLeftWheel, _frontLeftTransform);
            UpdateWheelPosition(_backRightWheel, _backRightTransform);
            UpdateWheelPosition(_frontRightWheel, _frontRightTransform);
        }

        private void UpdateWheelPosition(WheelCollider wheel, Transform wheelTransform)
        {
            wheel.GetWorldPose(out var pos, out var rot);

            wheelTransform.position = pos;
            wheelTransform.rotation = rot;
        }

        private void HandleBrakes()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _brakeStartTime = Time.time;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                _frontLeftWheel.brakeTorque = Mathf.Lerp(0f, _brakeMaxForce, (Time.time - _brakeStartTime) / _brakeDuration);
                _frontRightWheel.brakeTorque = Mathf.Lerp(0f, _brakeMaxForce, (Time.time - _brakeStartTime) / _brakeDuration);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                _frontLeftWheel.brakeTorque = 0f;
                _frontRightWheel.brakeTorque = 0f;
            }
        }

        private void ValidatePosition()
        {
            var upDot = Vector3.Dot(transform.up, Vector3.up);

            if (upDot <= 0f && _fellOverAt < 0)
            {
                _fellOverAt = Time.time;
            }
            else if (_fellOverAt > 0 && Time.time - _fellOverAt > _fallOverResetTimeout)
            {
                _fellOverAt = -1;

                transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            }
        }
    }
}