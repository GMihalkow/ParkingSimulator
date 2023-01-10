namespace ParkingSimulator.Utilities
{
    using UnityEngine;

    public class PrivateSingleton<T> : MonoBehaviour
    {
        public static T instance => _instance;

        protected static T _instance;

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this.GetComponent<T>();
        }
    }
}