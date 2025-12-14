//probably the single meaningful thing i've ever actually created on my own. let's go.
using System;
using System.Collections;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public string AbilityName;
    public float AbilityCooldownDuration;
    public float AbilityEffectDuration;

    //used to check if an ability is active.
    public bool IsActive = false;

    protected GameObject _player;

    public event Action AbilityActivated;

    /// <summary>
    /// Invoked when an ability has been successfully activated. <br/><br/>
    /// If an ability is interrupted early, that still counts as the ability being finished.
    /// </summary>
    public event Action AbilityFinished;

    /// <summary>
    /// Invoked when an ability is canceled. <br/><br/>
    /// Canceling an ability refers to stopping the ability entirely without incurring a cooldown. It's as if the ability never happened.
    /// If a game mechanic causes an ability to be interrupted, that ability is not canceled.
    /// </summary>
    public event Action AbilityCanceled; 

    protected float _timeWhenAbilityNextUsable = 0;

    /// <summary>
    /// Called when abilities are loaded onto the player
    /// </summary>
    public virtual void Init()
    {
        _player = GameManager.Instance.Player.gameObject;
    }

    /// <summary>
    /// attempts to use the ability. checks cooldown and if there's an ability currently active
    /// </summary>
    public void TryUse()
    {
        if (_timeWhenAbilityNextUsable > Time.time) return;

        //check the player's list of abilities to see if one is active. can't interrupt an ability. 
        foreach (var a in _player.GetComponent<PlayerController>().Abilities)
        {
            if (a.Value.IsActive) return;
        }

        _timeWhenAbilityNextUsable = Time.time + AbilityCooldownDuration + AbilityEffectDuration;
        AbilityActivated?.Invoke();
        _player.GetComponent<PlayerMovement>().SetGliding(false);
        Effect(true);
    }

    //doCooldown feels ugly to me but it's the cleanest way i can think of to do cooldowns only sometimes.
    //and i won't do cooldown if Effect is called by something else. Say there's an ability that lets you dash three times in a row, for example.
    //note to self: you can override as many times as you would like.
    /// <summary>
    /// base effect method including event invocation and cooldown routine. to be called after the ability is concluded.
    /// </summary>
    /// <param name="doCooldown">Whether or not this ability has a cooldown</param>
    public virtual void Effect(bool doCooldown)
    {
        _player.GetComponent<PlayerMovement>().SetGliding(true);
        AbilityFinished?.Invoke();
        if(!doCooldown) _timeWhenAbilityNextUsable -= AbilityCooldownDuration; //bit of a bandaid solution but i need to account for if the ability is used without cooldown
    }

    public void CancelAbility()
    {
        _player.GetComponent<PlayerMovement>().SetGliding(true);
        AbilityCanceled?.Invoke();
    }

    public void ResetCooldown()
    {
        _timeWhenAbilityNextUsable = Time.time;    
    }
}

public enum AbilityNames
{
    bounce,
    dash,
    parry
}