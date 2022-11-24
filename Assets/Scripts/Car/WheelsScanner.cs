namespace ParkingSimultaor.Car
{
    using UnityEngine;

    public class WheelsScanner : MonoBehaviour
    {
        [SerializeField] private Transform[] _wheels;
        [SerializeField, Min(0f)] private float _checkFrequency = 0.02f;
        [SerializeField] private LayerMask _targetLayers;
        [SerializeField] private bool _drawGizmos;
        [SerializeField, Min(0f)] private float _maxRayLength = 1f;

        private float _lastCheck;
        private WheelAreaPair[] _wheelAreaPairs;

        private void Awake() => _wheelAreaPairs = new WheelAreaPair[_wheels.Length];

        private void Update()
        {
            if (_checkFrequency + _lastCheck > Time.time) return;

            _lastCheck = Time.time;

            foreach (var wheel in _wheels)
            {
                var hasHit = Physics.Raycast(wheel.position, Vector3.down, out var hitInfo, _maxRayLength, _targetLayers);

                if (hasHit)
                {
                    var exists = FindWheelIndex(wheel) >= 0;

                    if (!exists)
                    {
                        var freeIndex = FindFreeIndex();
                    
                        if (freeIndex >= 0)
                        {
                            _wheelAreaPairs[freeIndex] = new WheelAreaPair(hitInfo.transform, wheel);
                        }
                    }
                }
                else
                {
                    var index = FindWheelIndex(wheel);

                    if (index >= 0)
                    {
                        _wheelAreaPairs[index] = null;
                    }
                }

                if (!_drawGizmos) continue;

                Debug.DrawRay(wheel.position, Vector3.down, Color.red, 5f);
            }
        }

        public bool IsScanSuccessful()
        {
            var areaTransform = default(Transform);
            var count = 0;

            foreach (var pair in _wheelAreaPairs)
            {
                if (pair == null) return false;

                if (pair.areaTransform != null)
                {
                    count++;
                }

                if (areaTransform == null)
                {
                    areaTransform = pair.areaTransform;
                    continue;
                }

                if (areaTransform != null && pair.areaTransform != areaTransform)
                {
                    return false;
                }
            }

            return count == _wheelAreaPairs.Length;
        }

        private int FindFreeIndex()
        {
            for (int index = 0; index < _wheelAreaPairs.Length; index++)
            {
                if (_wheelAreaPairs[index] == null || _wheelAreaPairs[index].wheelTransform == null || 
                    _wheelAreaPairs[index].areaTransform == null) return index;
            }

            return -1;
        }

        private int FindWheelIndex(Transform wheelTransform)
        {
            for (int index = 0; index < _wheelAreaPairs.Length; index++)
            {
                if (_wheelAreaPairs[index]?.wheelTransform == wheelTransform) return index;
            }

            return -1;
        }
    }
}