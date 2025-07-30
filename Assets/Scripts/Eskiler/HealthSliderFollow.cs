using UnityEngine;
using UnityEngine.UI;

public class HealthSliderFollow : MonoBehaviour
{
    [Header("Drone References")]
    public Transform playerDrone;
    public Transform enemyDrone;

    [Header("UI Elements (only RectTransforms)")]
    public RectTransform playerHealthUI;
    public RectTransform enemyHealthUI;

    [Header("Health Sliders")]
    public Slider playerHealthSlider;
    public Slider enemyHealthSlider;

    [Header("Settings")]
    public Camera mainCamera;
    public float fixedHeightOffset = 3f; // ka� birim yukar�da g�z�ks�n
    public float uiScale = 1f;           // UI objesinin boyutu (genellikle 1)

    private void Start()
    {
        SetupUITransform(playerHealthUI);
        SetupUITransform(enemyHealthUI);
    }

    private void SetupUITransform(RectTransform ui)
    {
        if (ui != null)
        {
            ui.localScale = Vector3.one * uiScale;
        }
    }

    private void Update()
    {
        UpdateUI(playerHealthUI, playerDrone);
        UpdateUI(enemyHealthUI, enemyDrone);
    }

    private void UpdateUI(RectTransform uiTransform, Transform target)
    {
        if (uiTransform == null || target == null || mainCamera == null)
            return;

        // Drone pozisyonuna g�re offset uygula
        Vector3 worldPos = target.position + Vector3.up * fixedHeightOffset;

        // Ekran pozisyonuna �evir
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        // UI'y� konumland�r
        uiTransform.position = screenPos;
    }
}
