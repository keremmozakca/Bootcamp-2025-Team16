using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button fireButton; // Ateþ butonu
    [SerializeField] private Slider healtSlider; // Slider UI elementi

    private DroneStats _droneStats;

    [Inject]
    public void Construct(DroneStats droneStats)
    {
        _droneStats = droneStats;
    }

    private void Start()
    {
        if (healtSlider != null && _droneStats != null)
        {
            healtSlider.minValue = 0f;
            healtSlider.maxValue = 1f;
            healtSlider.value = _droneStats.CurrentHealth / _droneStats.MaxHealth;
        }
    }

    private void OnEnable()
    {
        EventManager.OnFireRequested += HandleFireRequested;
        EventManager.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        EventManager.OnFireRequested -= HandleFireRequested;
    }

    public void OnFireButtonPressed()
    {
        EventManager.FireRequested();
    }
    private void HandleFireRequested()
    {
        Debug.Log("Fire button pressed!");
    }

    private void UpdateHealthBar(HealthChanedEvent healthChangedEvent)
    {
        if (healtSlider != null)
        {
            Debug.Log($"Updating health bar: Current Health = {healthChangedEvent.CurrentHealth}, Max Health = {healthChangedEvent.MaxHealth}");
            healtSlider.value = healthChangedEvent.CurrentHealth / healthChangedEvent.MaxHealth;
        }
        else
        {
            Debug.LogWarning("Health slider is not assigned in the UIManager.");
        }
    }
}
