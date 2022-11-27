namespace ParkingSimultaor.Control
{
    using ParkingSimultaor.Car;
    using ParkingSimultaor.Scripts.UI;
    using ParkingSimultaor.Utilities;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CarController _carController;
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private string _mainMenuScene = "MenuScene";
        [SerializeField] private ObjectScanner _parkingSlotsScanner;
        [SerializeField] private Image _marker;
        [SerializeField] private float _updateMarkerFrequency = 0.02f;

        private float _lastMarkerUpdate = -1;
        private Vector2 _input;
        private Camera _mainCamera;
        private bool _showHelperMarkers;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _carController.Initialize(transform);
            _pauseMenu.onResumeBtnClick += OnPauseMenuResumeBtnClick;
            _pauseMenu.onExitBtnClick += OnPauseMenuExitBtnClick;
        }

        private void Start() => Cursor.lockState = CursorLockMode.Locked;

        private void Update()
        {
            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            _carController.OnUpdate(ref _input);

            if (Input.GetKeyDown(KeyCode.E) && _carController.IsScanSuccessful)
            {
                Debug.Log("PARKING SUCCESSFUL!");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPauseMenuResumeBtnClick();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                _showHelperMarkers = !_showHelperMarkers;

                if (!_showHelperMarkers)
                {
                    _marker.gameObject.SetActive(false);
                }
            }

            if (_showHelperMarkers && (_updateMarkerFrequency < 0 || (Time.time - _lastMarkerUpdate > _updateMarkerFrequency)))
            {
                PlaceMarker();
            }
        }

        private void FixedUpdate() => _carController.OnFixedUpdate(ref _input);

        private void PlaceMarker()
        {
            if (_parkingSlotsScanner.results.Length > 0)
            {
                var flatCameraPos = _mainCamera.transform.position;
                flatCameraPos.y = 0f;

                var flatDirection = _mainCamera.transform.forward;
                flatDirection.y = 0f;

                var finalIndex = 0;
                var closestDot = default(float?);

                for (int index = 0; index < _parkingSlotsScanner.results.Length; index++)
                {
                    if (_parkingSlotsScanner.results[index] == null) continue;

                    var colliderFlatPos = _parkingSlotsScanner.results[index].transform.position;
                    colliderFlatPos.y = 0f;

                    var flatDirTowardsTarget = (colliderFlatPos - flatCameraPos).normalized;
                    var dot = Vector3.Dot(flatDirection, flatDirTowardsTarget);

                    if (!closestDot.HasValue)
                    {
                        closestDot = dot;
                        finalIndex = index;
                    }
                    else if (closestDot < dot)
                    {
                        closestDot = dot;
                        finalIndex = index;
                    }
                }

                _marker.gameObject.SetActive(true);
                _marker.transform.position = _mainCamera.WorldToScreenPoint(_parkingSlotsScanner.results[finalIndex].transform.position);
            }
            else
            {
                _marker.gameObject.SetActive(false);
            }

            _lastMarkerUpdate = Time.time;
        }

        private void OnPauseMenuResumeBtnClick()
        {
            _pauseMenu.gameObject.SetActive(!_pauseMenu.gameObject.activeSelf);

            Time.timeScale = _pauseMenu.gameObject.activeSelf ? 0f : 1f;
            Cursor.lockState = _pauseMenu.gameObject.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void OnPauseMenuExitBtnClick()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(_mainMenuScene, LoadSceneMode.Single);
        }
    }
}