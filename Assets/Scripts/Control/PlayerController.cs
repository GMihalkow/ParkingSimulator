namespace ParkingSimultaor.Control
{
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

        private Vector2 _input;

        private void FixedUpdate()
        {
            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

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
    }
}