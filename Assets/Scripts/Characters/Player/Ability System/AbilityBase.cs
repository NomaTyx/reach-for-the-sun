using System;
using System.Collections;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    public string abilityName;
    public float cooldown;
    public float duration;
    protected bool canUse = true;
    protected GameObject _player;

    public event Action<AbilityBase> AbilityActivated;
    public event Action<AbilityBase> AbilityFinished;

    protected WaitForSecondsRealtime cooldownWFS;
    //i'm probably going to create instances of all the abilities on start() of playercombatactions and add em to a list or smth

    public virtual void Init(GameObject player)
    {
        _player = player;
    }
    public void TryUse()
    {
        if (!canUse) return;
        canUse = false;
        Effect(true);
    }
    //doCooldown feels ugly to me but it's the cleanest way i can think of to do cooldowns only sometimes.
    //and i won't do cooldown if Effect is called by something else. Say there's an ability that lets you dash three times in a row, for example.
    //note to self: you can override as many times as you would like.
    public virtual void Effect(bool doCooldown)
    {
        if(doCooldown) StartCoroutine(Cooldown());
    }

    public IEnumerator Cooldown()
    {
        Debug.Log("cooling down" + abilityName);
        yield return cooldownWFS;
        canUse = true;
    }
}
