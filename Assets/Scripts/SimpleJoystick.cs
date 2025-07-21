using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Joystick Ayarlarý")]
    public RectTransform background;  // Joystick arka planý (UI Image)
    public RectTransform handle;      // Joystick tutma noktasý (UI Image)
    [Range(0.1f, 1f)]
    public float handleRange = 0.5f;  // Handle'ýn ne kadar uzaða gidebileceði

    [HideInInspector] public Vector2 Direction;

    private Vector2 _inputVector;

    void Start()
    {
        // Baþlangýçta sýfýrla
        Direction = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Joystick býrakýldýðýnda sýfýrla
        Direction = Vector2.zero;
        _inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        // Ekran koordinatlarýný joystick'in yerel koordinatlarýna çevir
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out position))
        {
            // Joystick'in boyutuna göre normalize et
            position.x = (position.x / background.sizeDelta.x);
            position.y = (position.y / background.sizeDelta.y);

            // -1 ile 1 arasýnda input vector oluþtur
            _inputVector = new Vector2(position.x * 2f, position.y * 2f);

            // Magnitude 1'i aþmasýn
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

            // Direction'ý güncelle
            Direction = _inputVector;

            // Handle'ý hareket ettir
            handle.anchoredPosition = new Vector2(
                _inputVector.x * (background.sizeDelta.x * handleRange),
                _inputVector.y * (background.sizeDelta.y * handleRange)
            );
        }
    }

    // Debug için
    void OnGUI()
    {
        if (Application.isPlaying)
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Direction: " + Direction.ToString("F2"));
        }
    }
}