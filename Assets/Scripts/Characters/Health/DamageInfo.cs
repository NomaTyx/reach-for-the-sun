using UnityEngine;

public class DamageInfo : MonoBehaviour
{
    public float Amount { get; private set; }
    public GameObject Instigator { get; private set; }
    public Health Target { get; private set; }

    public DamageInfo(float amount, GameObject instigator, Health target)
    {
        Target = target;
        Amount = amount;
        Instigator = instigator;
    }
}
