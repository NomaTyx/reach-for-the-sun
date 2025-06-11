//probably the single meaningful thing i've ever actually created on my own. let's go.
using System;
using System.Collections;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    public string AbilityName;
    public float CooldownDuration;
    public float EffectDuration;

    //used to check if an ability is active.
    public bool IsActive = false;

    protected bool _canUse = true;
    protected GameObject _player;
    protected AbilityData _abilityData; //just edit the scriptable objects to change data

    public event Action<AbilityBase> AbilityActivated;
    public event Action<AbilityBase> AbilityFinished;

    protected WaitForSecondsRealtime cooldownWFS;

    //i'm probably going to create instances of all the abilities on start() of playercombatactions and add em to a list or smth
    public virtual void Init()
    {
        _player = GameManager.Instance.Player.gameObject;
        _abilityData = GameManager.Instance.AbilityData;
    }
    public void TryUse()
    {
        if (!_canUse) return;

        //check the player's list of abilities to see if one is active. can't interrupt an ability. 
        foreach (var a in _player.GetComponent<PlayerController>().Abilities)
        {
            if (a.Value.IsActive) return;
        }

        AbilityActivated?.Invoke(this);
        _canUse = false;
        Effect(true);
    }
    //doCooldown feels ugly to me but it's the cleanest way i can think of to do cooldowns only sometimes.
    //and i won't do cooldown if Effect is called by something else. Say there's an ability that lets you dash three times in a row, for example.
    //note to self: you can override as many times as you would like.
    /// <summary>
    /// base effect method including event invocation and cooldown routine. to be called after the ability is concluded.
    /// </summary>
    /// <param name="doCooldown"></param>
    public virtual void Effect(bool doCooldown)
    {
        if(doCooldown) StartCoroutine(Cooldown());
        AbilityFinished?.Invoke(this);
    }

    //perhaps put this on the player and remove 
    public IEnumerator Cooldown()
    {
        Debug.Log("cooling down" + AbilityName);
        yield return cooldownWFS;
        _canUse = true;
    }
}

public enum AbilityNames
{
    bounce,
    dash,
    parry
}