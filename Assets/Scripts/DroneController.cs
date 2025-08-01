using UnityEngine;
using DG.Tweening;

public class DroneController : MonoBehaviour
{
    [Header("Touch Settings")]
    [SerializeField] private float swipeThreshold = 50f;

    [Header("Movement Settings")]
    [SerializeField] public static float ForwardSpeed = 10f;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float laneStep = 3f;
    [SerializeField] private float heightStep = 8f;

    [Header("Tilt Settings")]
    [SerializeField] private float tiltAngle = 10f;
    [SerializeField] private float tiltResetDuration = 0.2f;

    private Vector2 startTouchPos, endTouchPos;
    private int currentLane = 0; // -1=left, 0=center, +1=right
    private int currentHeight = 0; // 0=top, -1=bottom
    private bool isMoving = false;

    private void Update()
    {
        HandleSwipeInput();
        // Sürekli ileri hareket (World Z ekseni)
        transform.Translate(Vector3.forward * ForwardSpeed * Time.deltaTime, Space.World);
    }

    private void HandleSwipeInput()
    {
        if (Input.touchCount == 0 || isMoving) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
            startTouchPos = touch.position;
        else if (touch.phase == TouchPhase.Ended)
        {
            endTouchPos = touch.position;
            Vector2 swipe = endTouchPos - startTouchPos;
            if (swipe.magnitude < swipeThreshold) return;

            if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
            {
                if (swipe.x > 0) MoveRight();
                else MoveLeft();
            }
            else
            {
                if (swipe.y > 0) MoveUp();
                else MoveDown();
            }
        }
    }

    private void MoveLeft()
    {
        if (currentLane <= -1) return;
        currentLane--;
        isMoving = true;
        // Sadece X ekseninde kaydýr
        transform.DOMoveX(transform.position.x - laneStep, moveDuration)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => isMoving = false);
        ApplyTilt(Vector3.forward * tiltAngle);
    }

    private void MoveRight()
    {
        if (currentLane >= 1) return;
        currentLane++;
        isMoving = true;
        transform.DOMoveX(transform.position.x + laneStep, moveDuration)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => isMoving = false);
        ApplyTilt(Vector3.back * tiltAngle);
    }

    private void MoveUp()
    {
        if (currentHeight >= 0) return;
        currentHeight++;
        isMoving = true;
        transform.DOMoveY(transform.position.y + heightStep, moveDuration)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => isMoving = false);
    }

    private void MoveDown()
    {
        if (currentHeight <= -1) return;
        currentHeight--;
        isMoving = true;
        transform.DOMoveY(transform.position.y - heightStep, moveDuration)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => isMoving = false);
    }

    private void ApplyTilt(Vector3 tiltEuler)
    {
        Quaternion originalRot = transform.rotation;
        Quaternion tiltRot = Quaternion.Euler(tiltEuler) * originalRot;

        transform.DORotateQuaternion(tiltRot, moveDuration)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() =>
                 {
                     transform.DORotateQuaternion(originalRot, tiltResetDuration)
                              .SetEase(Ease.OutQuad);
                 });
    }
}
