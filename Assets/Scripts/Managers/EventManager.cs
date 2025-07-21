using System;
public class EventManager
{
    public static event Action OnFireRequested;
    public static event Action<DamageEvent> OnDamageRequested;
    public static event Action<HealthChanedEvent> OnHealthChanged;
    public static event Action<IDamagable> OnEntityDestroyed;

    public static void FireRequested()
    {
        OnFireRequested?.Invoke();
    }

    public static void RequestDamage(DamageEvent damageEvent)
    {
        OnDamageRequested?.Invoke(damageEvent);
    }

    public static void NotifyHealthChanged(HealthChanedEvent healthChangedEvent)
    {
        OnHealthChanged?.Invoke(healthChangedEvent);
    }

    public static void NotifyEntityDestroyed(IDamagable entity)
    {
        OnEntityDestroyed?.Invoke(entity);
    }

}
