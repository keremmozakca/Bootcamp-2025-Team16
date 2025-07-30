using UnityEngine;

public class WingRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 1000f; // Rotation speed in degrees per second

    private void Update()
    {
        // Rotate the wings around the local X-axis
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
