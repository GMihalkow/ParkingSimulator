namespace ParkingSimulator.Car
{
    using System;
    using UnityEngine;

    [Serializable]
    public class CarController
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

        public bool IsScanSuccessful => _wheelsScanner.IsScanSuccessful();

        private Transform _carTransform;
        private float _brakeStartTime;
        private float _fellOverAt = -1;

        public void Initialize(Transform carTransform) => _carTransform = carTransform;

        public void OnUpdate(ref Vector2 input)
        {
            HandleBrakes();
            ValidatePosition();
        }

        public void OnFixedUpdate(ref Vector2 input)
        {
            var turnAngle = _maxTurnAngle * input.x;

            _frontLeftWheel.steerAngle = turnAngle;
            _frontRightWheel.steerAngle = turnAngle;

            _frontLeftWheel.motorTorque = input.y * _maxTorque;
            _frontRightWheel.motorTorque = input.y * _maxTorque;

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
            var upDot = Vector3.Dot(_carTransform.up, Vector3.up);

            if (upDot <= 0f && _fellOverAt < 0)
            {
                _fellOverAt = Time.time;
            }
            else if (_fellOverAt > 0 && Time.time - _fellOverAt > _fallOverResetTimeout)
            {
                _fellOverAt = -1;

                _carTransform.rotation = Quaternion.LookRotation(_carTransform.forward, Vector3.up);
            }
        }
    }
}