namespace ParkingSimultaor.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "MainScene";

        /// <summary>
        /// Called from editor script
        /// </summary>
        public void OnPlayBtnClick() => SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);

        /// <summary>
        /// Called from editor script
        /// </summary>
        public void OnExitBtnClick() => Application.Quit();
    }
}