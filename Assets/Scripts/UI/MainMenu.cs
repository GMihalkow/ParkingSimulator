namespace ParkingSimulator.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "MainScene";

        private bool _playBtnIsClicked;

        /// <summary>
        /// Called from editor script
        /// </summary>
        public void OnPlayBtnClick()
        {
            if (_playBtnIsClicked) return;

            _playBtnIsClicked = true;
            SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
        }

        /// <summary>
        /// Called from editor script
        /// </summary>
        public void OnExitBtnClick() => Application.Quit();
    }
}