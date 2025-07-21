using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Joystick Ayarlar�")]
    public RectTransform background;  // Joystick arka plan� (UI Image)
    public RectTransform handle;      // Joystick tutma noktas� (UI Image)
    [Range(0.1f, 1f)]
    public float handleRange = 0.5f;  // Handle'�n ne kadar uza�a gidebilece�i

    [HideInInspector] public Vector2 Direction;

    private Vector2 _inputVector;

    void Start()
    {
        // Ba�lang��ta s�f�rla
        Direction = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Joystick b�rak�ld���nda s�f�rla
        Direction = Vector2.zero;
        _inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        // Ekran koordinatlar�n� joystick'in yerel koordinatlar�na �evir
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out position))
        {
            // Joystick'in boyutuna g�re normalize et
            position.x = (position.x / background.sizeDelta.x);
            position.y = (position.y / background.sizeDelta.y);

            // -1 ile 1 aras�nda input vector olu�tur
            _inputVector = new Vector2(position.x * 2f, position.y * 2f);

            // Magnitude 1'i a�mas�n
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

            // Direction'� g�ncelle
            Direction = _inputVector;

            // Handle'� hareket ettir
            handle.anchoredPosition = new Vector2(
                _inputVector.x * (background.sizeDelta.x * handleRange),
                _inputVector.y * (background.sizeDelta.y * handleRange)
            );
        }
    }

    // Debug i�in
    void OnGUI()
    {
        if (Application.isPlaying)
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Direction: " + Direction.ToString("F2"));
        }
    }
}