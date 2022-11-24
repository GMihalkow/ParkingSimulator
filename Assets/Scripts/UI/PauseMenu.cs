namespace ParkingSimultaor.Scripts.UI
{
    using System;
    using UnityEngine;

    public class PauseMenu : MonoBehaviour
    {
        public event Action onResumeBtnClick;
        public event Action onExitBtnClick;

        /// <summary>
        /// Called from editor script
        /// </summary>
        public void OnResumeBtnClick() => onResumeBtnClick?.Invoke();

        /// <summary>
        /// Called from editor script
        /// </summary>
        public void OnExitBtnClick() => onExitBtnClick?.Invoke();
    }
}