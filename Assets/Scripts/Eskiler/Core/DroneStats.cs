using UnityEngine;

public class DroneStats : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;
    [SerializeField] public float Speed = 5f; // Drone'un hareket h�z�    

    //PowerMultiplier ilerde field olarak kullan�labilir bak�cam
    public float powerMultiplier = 1.0f; //Drone'un ne kadar g��l� oldu�unu belirleyecek
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;

    private void Start()
    {
        _currentHealth = _maxHealth; // Ba�lang��ta tam sa�l�k

        EventManager.NotifyHealthChanged(new HealthChanedEvent(this, _currentHealth, _maxHealth));
    }

    public void ReceiveDamage(float amount, object instigator)
    {
        float otherDronePower = 1f;
        if (instigator is GameObject go)
        {
            var otherDroneStats = go.GetComponent<DroneStats>();
            if (otherDroneStats != null)
            {
                otherDronePower = otherDroneStats.powerMultiplier;
            }
            else
            {
                Debug.Log("Instigator does not have DroneStats component.");
            }
        }

        float appliedDamage = amount * otherDronePower; // di�er Drone'un g�� katsay�s�na g�re hasar uygula
        _currentHealth = Mathf.Clamp(_currentHealth - appliedDamage, 0, _maxHealth);

        EventManager.NotifyHealthChanged(new HealthChanedEvent(this, _currentHealth, _maxHealth));
        if (_currentHealth <= 0)
        {
            DestroyTheDrone();
        }
    }

    private void DestroyTheDrone()
    {
        // Drone yok edildi�inde yap�lacak i�lemler
        Debug.Log("Drone destroyed.");
        EventManager.NotifyEntityDestroyed(this); // Olay y�neticisine yok edildi�ini bildir
        Destroy(gameObject); // Drone'u yok et
    }
}
