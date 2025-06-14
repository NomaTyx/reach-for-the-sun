using System.Collections;
using UnityEngine;

public class AbilityBounce : AbilityBase
{
    //hardcoded values for now
    private float _bounceRange = 10;
    private float _bounceForce = 100;
    private float _bounceCooldown = 2f;

    private SphereCollider _bounceCollider;
    private Rigidbody _rb;

    public override void Init()
    {
        base.Init();
        AbilityName = "Bounce";
        _rb = _player.GetComponent<Rigidbody>();
        //hardcoding cooldowns and durations for now, until i can decide where to store them
        CooldownDuration = _bounceCooldown; 
        EffectDuration = 0;
        _bounceCollider = _player.GetComponent<SphereCollider>();
        _bounceCollider.radius = _bounceRange;
        
        cooldownWFS = new WaitForSecondsRealtime(CooldownDuration);
    }

    public override void Effect(bool doCooldown) 
    {
        float numOfEnemies = 0;

        foreach (Collider c in Physics.OverlapSphere(transform.position, 10))
        {
            Health hitHealth = c.GetComponent<Health>();
            if (hitHealth == null) continue;
            if (hitHealth.gameObject == _player) continue;
            hitHealth.Damage(new DamageInfo(1, this.gameObject, hitHealth.gameObject));

            //currently unused variable that tracks how many enemies are being bounced off of.
            numOfEnemies++;
        }

        if (numOfEnemies == 0)
        {
            CancelAbility();
            return;
        }

        //later on i will factor in the player's x and y velocity
        Vector3 prevVelocity = _rb.linearVelocity;
        _rb.linearVelocity = Vector3.zero;

        //add more complex logic potentially
        _rb.AddForce(Vector3.up * (_bounceForce + numOfEnemies), ForceMode.Impulse);

        base.Effect(doCooldown);
    }
}
