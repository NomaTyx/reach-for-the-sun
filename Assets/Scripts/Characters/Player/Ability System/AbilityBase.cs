using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    public string abilityName;
    public float cooldown;
    public float duration;

    public abstract void Effect();
}
