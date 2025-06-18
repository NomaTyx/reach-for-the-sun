using UnityEngine;

/// <summary>
/// Create a new instance of DamageInfo whenever you deal damage. It's a future-proofing thing, in case i want to add a thorns effect for example
/// </summary>
public class DamageInfo
{
    public float Amount { get; private set; }
    public GameObject Instigator { get; private set; }
    public GameObject Target { get; private set; }

    public DamageInfo(float amount, GameObject instigator, GameObject target)
    {
        Target = target;
        Amount = amount;
        Instigator = instigator;
    }
}
