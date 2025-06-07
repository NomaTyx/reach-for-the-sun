using System.Collections;
using UnityEngine;

public class AbilityBounce : AbilityBase
{
    private SphereCollider _bounceCollider;

    //hardcoded values for now
    private float _bounceRange = 10;
    private float _bounceForce = 100;
    private float _bounceCooldown = 5f;
    private Rigidbody _rb;

    public override void Init()
    {
        base.Init();
        AbilityName = "Bounce";
        _rb = _player.GetComponent<Rigidbody>();
        //hardcoding cooldowns and durations for now, until i can decide where to store them
        CooldownDuration = 2f; 
        EffectDuration = 0;
        _bounceCollider = _player.GetComponent<SphereCollider>();
        _bounceCollider.radius = _bounceRange;
        
        cooldownWFS = new WaitForSecondsRealtime(CooldownDuration);
    }

    public override void Effect(bool doCooldown) 
    {
        Debug.Log("succeeded bounce");
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

        if (numOfEnemies == 0) return;

        //later on i will factor in the player's x and y velocity
        Vector3 prevVelocity = _rb.linearVelocity;
        _rb.linearVelocity = Vector3.zero;

        //add more complex logic potentially
        _rb.AddForce(Vector3.up * (_bounceForce + numOfEnemies), ForceMode.Impulse);

        //OnBounce?.Invoke(_bounceCooldown);

        base.Effect(doCooldown);
    }
}
