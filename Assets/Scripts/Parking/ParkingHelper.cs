namespace ParkingSimultaor.Parking
{
    using ParkingSimultaor.Parking.Enums;
    using UnityEngine;

    public class ParkingHelper : MonoBehaviour
    {
        [SerializeField] private Type _type;
        [SerializeField] private Texture2D _helpImage;

        //private void Update()
        //{
        //    if (_type != Type.Horizontal) return;

        //    var rot = Quaternion.AngleAxis(45f, Vector3.up);
        //    Debug.DrawRay(transform.position, rot * transform.forward * 10f, Color.red);
        //}
    }
}