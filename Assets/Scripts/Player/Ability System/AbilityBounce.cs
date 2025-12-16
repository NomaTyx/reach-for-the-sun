using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBounce : Ability
{
    //hardcoded values for now
    private float _bounceRange = 10;
    private float _bounceForce = 20;
    private float _bounceCooldown = 2f;

    private SphereCollider _bounceCollider;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;

    private List<Health> _bouncedEnemies;

    public override void Init()
    {
        base.Init();
        AbilityName = "Bounce";
        //hardcoding cooldowns and durations for now, until i can decide where to store them
        AbilityCooldownDuration = _bounceCooldown; 
        AbilityEffectDuration = 0;

        _rb = _player.GetComponent<Rigidbody>();
        _bounceCollider = _player.GetComponent<SphereCollider>();
        _playerMovement = _player.GetComponent<PlayerMovement>();

        _bounceCollider.radius = _bounceRange;
    }

    public override void TryUse()
    {
        _bouncedEnemies = new List<Health>();
        //TODO: make this only check the enemy layer
        foreach (Collider c in Physics.OverlapSphere(transform.position, _bounceCollider.radius))
        {
            Health hitHealth = c.GetComponent<Health>();
            if (hitHealth == null) continue;
            if (hitHealth.gameObject == _player) continue;

            //currently unused variable that tracks how many enemies are being bounced off of.
            _bouncedEnemies.Add(hitHealth);
        }

        if (_bouncedEnemies.Count > 0)
        {
            base.TryUse();
        }
    }

    /// <summary>
    /// look for every enemy within bouncing range and deal damage to it. also propel with additional force per enemy
    /// </summary>
    public override void Effect(bool doCooldown) 
    {
        foreach (Health hitHealth in _bouncedEnemies)
        {
            hitHealth.Damage(new DamageInfo(1, gameObject, hitHealth.gameObject));
        }

        //later on i will factor in the player's x and y velocity
        Vector3 prevVelocity = _rb.linearVelocity;
        _rb.linearVelocity = Vector3.zero;

        //add more complex logic potentially
        _playerMovement.SetPlayerVelocity(new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z));
        _playerMovement.AddForceToPlayer(Vector3.up * (_bounceForce + _bouncedEnemies.Count), ForceMode.Impulse);

        base.Effect(doCooldown);
    }
}
