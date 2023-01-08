namespace ParkingSimulator.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class WinMenu : MonoBehaviour
    {
        [SerializeField] private string _menuSceneName = "MenuScene";
        [SerializeField] private string _mainGameSceneName = "MainScene";

        public event Action onRestartBtnClick;
        public event Action onExitBtnClick;

        /// <summary>
        /// Called by editor script
        /// </summary>
        public void OnRestartBtnClick()
        {
            onRestartBtnClick?.Invoke();
            SceneManager.LoadScene(_mainGameSceneName, LoadSceneMode.Single);
        }

        /// <summary>
        /// Called by editor script
        /// </summary>
        public void OnExitBtnClick()
        {
            onExitBtnClick?.Invoke();
            SceneManager.LoadScene(_menuSceneName, LoadSceneMode.Single);
        }
    }
}