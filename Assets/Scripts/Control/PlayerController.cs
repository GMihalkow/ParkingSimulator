namespace ParkingSimultaor.Control
{
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TiresController _tiresController;
        [SerializeField] private float _speed = 5f;

        private Vector3 _flatDirection;

        private void Awake()
        {
            _flatDirection = new Vector3(1f, 0f, 1f);
            _tiresController.isActivated = true;
        }

        private void Update()
        {
            var horizontalAxis = Input.GetAxis("Horizontal");
            var verticalAxis = Input.GetAxis("Vertical");

            var dir = _flatDirection;
            dir.x *= horizontalAxis;
            dir.z *= verticalAxis;

            transform.Translate(dir);
        }
    }
}