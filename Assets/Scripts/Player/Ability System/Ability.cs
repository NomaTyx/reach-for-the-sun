//probably the single meaningful thing i've ever actually created on my own. let's go.
using System;
using System.Collections;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public string AbilityName { get; protected set; }
    public float AbilityCooldownDuration { get; protected set; }
    public float AbilityEffectDuration { get; protected set; }

    /// <summary>
    /// used to check if an ability is currently in-use (some abilities happen instantly, others take a certain amount of time)
    /// </summary>
    public bool IsActive { get; protected set; } = false;

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
    /// Attempts to use the ability. Checks cooldown and if there's an ability currently active. <br/><br/>
    /// Some abilities have preconditions required. Those functions will override TryUse and check their own preconditions, but <em>must</em> call the base TryUse afterwards.
    /// </summary>
    public virtual void TryUse()
    {
        if (_timeWhenAbilityNextUsable > Time.time) return;

        foreach (var a in _player.GetComponent<PlayerController>().Abilities)
        {
            if (a.Value.IsActive) return;
        }

        _timeWhenAbilityNextUsable = Time.time + AbilityCooldownDuration + AbilityEffectDuration;
        AbilityActivated?.Invoke();
        _player.GetComponent<PlayerMovement>().SetGliding(false);
        Effect(true);
    }

    //doCooldown feels ugly to me but it's the cleanest way i can think of to sometimes not do cooldowns.
    //and i won't do cooldown if Effect is called by something else. Say there's an ability that lets you dash three times in a row, for example.
    //note to self: you can override as many times as you would like.
    /// <summary>
    /// base effect method including event invocation and cooldown routine. <em>Must</em> be called after the ability is concluded.
    /// </summary>
    /// <param name="doCooldown">Whether or not this ability has a cooldown</param>
    public virtual void Effect(bool doCooldown)
    {
        _player.GetComponent<PlayerMovement>().SetGliding(true);
        AbilityFinished?.Invoke();
        if(!doCooldown) _timeWhenAbilityNextUsable -= AbilityCooldownDuration; //bit of a bandaid solution but i need to account for if the ability is used without cooldown
    }

    /// <summary>
    /// Method to be called if an effect (the player being teleported somewhere, for example) wants to override the function of an ability.
    /// </summary>
    public void CancelAbility()
    {
        _player.GetComponent<PlayerMovement>().SetGliding(true);
        AbilityCanceled?.Invoke();
    }

    /// <summary>
    /// Replenishes the cooldown and makes the ability instantly usable again. Useful for a cooldown item, for example.
    /// </summary>
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