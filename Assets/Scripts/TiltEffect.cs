using UnityEngine;

public class TiltEffect : MonoBehaviour
{
    [SerializeField] private Transform _modelTransform;

    [Tooltip("Eðim Açýsý")]
    [SerializeField] private float _maxTiltAngle = 15f;

    [Tooltip("Eðimin Deðiþim Hýzý")]
    [SerializeField] private float _tiltSpeed = 5f;

    private Vector3 _targetTiltEuler = Vector3.zero;
    private bool _useFixedDeltaTime = false;

    // Hangi update döngüsünde çaðrýldýðýný belirtmek için
    public void SetFixedTimeMode(bool useFixed)
    {
        _useFixedDeltaTime = useFixed;
    }

    public void UpdateTilt(Vector2 inputDirection)
    {
        if (inputDirection.magnitude > 0.1f)
        {
            float tiltX = -inputDirection.y * _maxTiltAngle;
            float tiltZ = inputDirection.x * _maxTiltAngle;
            _targetTiltEuler = new Vector3(tiltX, 0f, tiltZ);
        }
        else
        {
            _targetTiltEuler = Vector3.zero;
        }

        float deltaTime = _useFixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime;

        // Sadece eðim için X-Z ekseni, Y yönünü deðiþtirme
        Quaternion targetTiltRotation = Quaternion.Euler(_targetTiltEuler);
        _modelTransform.localRotation = Quaternion.Slerp(_modelTransform.localRotation, targetTiltRotation, deltaTime * _tiltSpeed);
    }

}