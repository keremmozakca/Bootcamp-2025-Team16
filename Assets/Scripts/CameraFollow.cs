using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target; // Drone referansý
    [SerializeField] private Vector3 _offset = new Vector3(0, 9.13f, -21.88f);
    [SerializeField] private float _followDuration = 0.2f;

    private void Update()
    {
        transform.position += Vector3.forward * DroneController.ForwardSpeed * Time.deltaTime;
    }
}
