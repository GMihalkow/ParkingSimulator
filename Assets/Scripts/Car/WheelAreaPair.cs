namespace ParkingSimultaor.Car
{
    using System;
    using UnityEngine;

    [Serializable]
    public class WheelAreaPair
    {
        [SerializeField] private Transform _areaTransform;
        [SerializeField] private Transform _wheelTransform;

        public Transform areaTransform => _areaTransform;
        public Transform wheelTransform => _wheelTransform;

        public WheelAreaPair(Transform areaTransform, Transform wheelTransform)
        {
            _areaTransform = areaTransform;
            _wheelTransform = wheelTransform;
        }
    }
}