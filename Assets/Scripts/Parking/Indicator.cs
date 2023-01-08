namespace ParkingSimulator.Parking
{
    using UnityEngine;

    public class Indicator : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _travelDistance = 2f;

        private Vector3 _initialPos;
        private float _angle;

        private void Awake() =>_initialPos = transform.position;

        private void Update()
        {
            if (_angle > 360f)
            {
                _angle -= 360f;
            }

            transform.position = _initialPos + Vector3.up * _travelDistance * Mathf.Sin(_angle);

            _angle += _speed * Time.deltaTime;
        }
    }
}