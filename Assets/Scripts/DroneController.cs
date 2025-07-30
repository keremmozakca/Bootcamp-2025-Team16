using UnityEngine;
using DG.Tweening; // DOTween'i kullanabilmek için

public class DroneController : MonoBehaviour
{
    private Vector2 _startTouchPos;
    private Vector2 _endTouchPos;

    public static float ForwardSpeed = 10f;
    private float _swipeThreshold = 50f;
    private float _moveDuration = 0.2f;

    private float _horizontalStep = 3f;
    private float _verticalStep = 8f;

    private int _currentLane = 0; // -1 = sol, 0 = orta, 1 = sað
    private int _currentHeight = 0; // 0 = yukarý, -1 = aþaðý

    private float _tiltAmount = 10f;
    private float _tiltResetDuration = 0.2f;

    private void Start()
    {
        // Pozisyon baþlangýcý 0 lane ve yukarýda
        Vector3 startPos = transform.position;
        transform.position = new Vector3(startPos.x, startPos.y, startPos.z);
    }

    private void Update()
    {
        HandleSwipeInput();

        // Z ekseninde ileri hareket
        transform.Translate(Vector3.forward * ForwardSpeed * Time.deltaTime);
    }

    private void HandleSwipeInput()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            _startTouchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            _endTouchPos = touch.position;
            Vector2 swipe = _endTouchPos - _startTouchPos;

            if (swipe.magnitude < _swipeThreshold)
                return;

            if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
            {
                if (swipe.x > 0)
                    MoveRight();
                else
                    MoveLeft();
            }
            else
            {
                if (swipe.y > 0)
                    MoveUp();
                else
                    MoveDown();
            }
        }
    }

    private void MoveLeft()
    {
        if (_currentLane > -1)
        {
            _currentLane--;
            float targetX = transform.position.x - _horizontalStep;
            transform.DOMoveX(targetX, _moveDuration).SetEase(Ease.OutQuad);
            ApplyTilt(Vector3.forward * _tiltAmount);
        }
    }

    private void MoveRight()
    {
        if (_currentLane < 1)
        {
            _currentLane++;
            float targetX = transform.position.x + _horizontalStep;
            transform.DOMoveX(targetX, _moveDuration).SetEase(Ease.OutQuad);
            ApplyTilt(Vector3.back * _tiltAmount);
        }
    }

    private void MoveUp()
    {
        if (_currentHeight < 0)
        {
            _currentHeight++;
            float targetY = transform.position.y + _verticalStep;
            transform.DOMoveY(targetY, _moveDuration).SetEase(Ease.OutQuad);
        }
    }

    private void MoveDown()
    {
        if (_currentHeight > -1)
        {
            _currentHeight--;
            float targetY = transform.position.y - _verticalStep;
            transform.DOMoveY(targetY, _moveDuration).SetEase(Ease.OutQuad);
        }
    }

    private void ApplyTilt(Vector3 tiltEuler)
    {
        transform.DORotate(tiltEuler, _moveDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DORotate(Vector3.zero, _tiltResetDuration).SetEase(Ease.OutQuad);
        });
    }
}
