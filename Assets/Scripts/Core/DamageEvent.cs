using UnityEngine;

public struct DamageEvent
{
    public IDamagable Target;
    public float Amount;
    public object Instigator; //GameObject veya ID verilecek belki Tag

    public DamageEvent(IDamagable target, float amount, object instigator)
    {
        Target = target;
        Amount = amount;
        Instigator = instigator;
    }
}
