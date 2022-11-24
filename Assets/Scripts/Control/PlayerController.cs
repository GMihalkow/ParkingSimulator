namespace ParkingSimultaor.Control
{
    using ParkingSimultaor.Car;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CarController _carController;

        private Vector2 _input;

        private void Awake() => _carController.Initialize(transform);

        private void Update()
        {
            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            _carController.OnUpdate(ref _input);

            if (Input.GetKeyDown(KeyCode.E) && _carController.IsScanSuccessful)
            {
                Debug.Log("PARKING SUCCESSFUL!");
            }
        }

        private void FixedUpdate() => _carController.OnFixedUpdate(ref _input);
    }
}