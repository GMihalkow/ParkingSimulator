namespace ParkingSimultaor.Control
{
    using ParkingSimultaor.Car;
    using ParkingSimultaor.Scripts.UI;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CarController _carController;
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private string _mainMenuScene = "MenuScene";

        private Vector2 _input;

        private void Awake()
        {
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
        }

        private void FixedUpdate() => _carController.OnFixedUpdate(ref _input);

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