using UnityEngine;

public class HealthChanedEvent
{
    public IDamagable Entity;
    public float CurrentHealth;
    public float MaxHealth;

    public HealthChanedEvent(IDamagable entity, float currentHealth, float maxHealth)
    {
        Entity = entity;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
}
