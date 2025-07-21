using UnityEngine;

public interface IDamagable
{
    void ReceiveDamage(float amount, object instigator);
    float CurrentHealth { get; }
    float MaxHealth { get;  }
}




