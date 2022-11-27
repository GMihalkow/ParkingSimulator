namespace ParkingSimultaor.Utilities
{
    using UnityEngine;

    public class ObjectScanner : MonoBehaviour
    {
        [SerializeField] private LayerMask _targetLayers;
        [SerializeField, Min(0f)] private float _scanRadius = 5f;
        [SerializeField] private bool _drawGizmos;
        [SerializeField, Min(0f)] private float _scanFrequency = 0.02f;
        [SerializeField, Min(0)] private int _maxResultsCount = 3;

        public Collider[] results => _results;

        private Collider[] _results;
        private float _lastScan = -1;

        private void Awake() => _results = new Collider[_maxResultsCount];

        private void Update()
        {
            if (_lastScan < 0 || (_lastScan >= 0f && Time.time - _lastScan > _scanFrequency))
            {
                Scan();
            }
        }

        private int Scan()
        {
            _lastScan = Time.time;

            return Physics.OverlapSphereNonAlloc(transform.position, _scanRadius, _results, _targetLayers);
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos) return;

            Gizmos.DrawWireSphere(transform.position, _scanRadius);
        }
    }
}