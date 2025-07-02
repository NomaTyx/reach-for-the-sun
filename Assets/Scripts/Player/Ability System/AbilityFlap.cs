using UnityEngine;

public class AbilityFlap : AbilityBase
{
    //hardcoded values for now
    private float _flapForce = 10f;
    private float _flapCooldown = 5f;
    
    private Rigidbody _rb;

    public override void Init()
    {
        base.Init();
        AbilityName = "Flap";
        _rb = _player.GetComponent<Rigidbody>();
        AbilityCooldownDuration = _flapCooldown;
        AbilityEffectDuration = 0;
    }

    public override void Effect(bool doCooldown)
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        _rb.AddForce(Vector3.up * _flapForce, ForceMode.Impulse);
        base.Effect(doCooldown);
    }
}
