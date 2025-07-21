using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Tooltip("Joystick referansı")]
    public SimpleJoystick joystick;

    [Header("Hareket Ayarları")]
    public float moveForce = 10f;
    public float maxSpeed;
    public float rotationSpeed = 200f;
    public float movementThreshold = 0.1f;
    public float dragCoefficient = 0.92f;
    public float stopThreshold = 0.1f;

    [Header("Tilt Ayarları")]
    public float maxTiltAngle = 15f;
    public float tiltSpeed = 5f;

    private Rigidbody _rb;
    private float _currentYAngle = 0f;
    private Vector3 _targetTiltEuler = Vector3.zero;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezePositionY;
        DroneStats _stat = new DroneStats();
        maxSpeed = _stat.Speed;
        _currentYAngle = transform.eulerAngles.y;
    }

    void FixedUpdate()
    {
        if (joystick == null) return;

        Vector2 input = joystick.Direction;
        bool hasInput = input.magnitude > movementThreshold;

        if (hasInput)
        {
            ApplyRotationAndTilt(input);
            ApplyMovement(input);
        }
        else
        {
            ApplyDrag();
            ApplyRotationAndTilt(Vector2.zero); // tilt sıfırlansın ama yön bozulmasın
        }

        LimitVelocity();
    }

    private void ApplyRotationAndTilt(Vector2 input)
    {
        // --- Y Ekseni Rotasyonu Hesapla ---
        if (input.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
            float deltaAngle = Mathf.DeltaAngle(_currentYAngle, targetAngle);
            _currentYAngle += deltaAngle * rotationSpeed * Time.fixedDeltaTime / 100f;
        }

        Quaternion rotationY = Quaternion.Euler(0f, _currentYAngle, 0f);

        // --- X-Z Tilt Hesapla ---
        float tiltX = -input.y * maxTiltAngle;
        float tiltZ = input.x * maxTiltAngle;
        Quaternion tiltRotation = Quaternion.Euler(tiltX, 0f, tiltZ);

        // --- Tilt + Dönüş Birleştir ---
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationY * tiltRotation, Time.fixedDeltaTime * tiltSpeed);
    }

    private void ApplyMovement(Vector2 input)
    {
        Vector3 moveDir = transform.forward * input.magnitude;
        _rb.AddForce(moveDir * moveForce, ForceMode.Force);
    }

    private void ApplyDrag()
    {
        Vector3 velocity = _rb.linearVelocity;
        velocity.x *= dragCoefficient;
        velocity.z *= dragCoefficient;

        if (Mathf.Abs(velocity.x) < stopThreshold) velocity.x = 0f;
        if (Mathf.Abs(velocity.z) < stopThreshold) velocity.z = 0f;

        _rb.linearVelocity = velocity;
    }

    private void LimitVelocity()
    {
        Vector3 velocity = _rb.linearVelocity;
        velocity.y = 0f;

        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            velocity.x = horizontalVelocity.x;
            velocity.z = horizontalVelocity.z;
        }

        _rb.linearVelocity = velocity;
    }
}
