namespace ParkingSimultaor.Control
{
    using ParkingSimultaor.Car;
    using ParkingSimultaor.UI;
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
        [SerializeField] private GameObject _helpersScreen;
        [SerializeField] private Image _invalidParkingPositionImage;
        [SerializeField] private WinMenu _winScreen;

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

            _winScreen.onRestartBtnClick += OnWinMenuRestartBtnClick;
            _winScreen.onExitBtnClick += OnWinMenuExitBtnClick;
        }

        private void Start() => Cursor.lockState = CursorLockMode.Locked;

        private void Update()
        {
            if (_winScreen.gameObject.activeSelf) return;

            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            _carController.OnUpdate(ref _input);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_carController.IsScanSuccessful)
                {
                    ToggleScreen(_winScreen.gameObject, true);
                    return;
                }
                else
                {
                    _invalidParkingPositionImage.gameObject.SetActive(true);
                }
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                _invalidParkingPositionImage.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_helpersScreen.gameObject.activeSelf)
                {
                    ToggleScreen(_helpersScreen, false);
                }
                else
                {
                    OnPauseMenuResumeBtnClick();
                }
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

            if (Input.GetKeyDown(KeyCode.Tab) && !_pauseMenu.gameObject.activeSelf)
            {
                ToggleScreen(_helpersScreen, !_helpersScreen.gameObject.activeSelf);
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

        private void OnPauseMenuResumeBtnClick() => ToggleScreen(_pauseMenu.gameObject, !_pauseMenu.gameObject.activeSelf);

        private void ToggleScreen(GameObject screen, bool active)
        {
            screen.gameObject.SetActive(active);

            Time.timeScale = screen.gameObject.activeSelf ? 0f : 1f;
            Cursor.lockState = screen.gameObject.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void OnPauseMenuExitBtnClick()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(_mainMenuScene, LoadSceneMode.Single);
        }

        private void OnWinMenuRestartBtnClick()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }

        private void OnWinMenuExitBtnClick()
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1f;
        }
    }
}