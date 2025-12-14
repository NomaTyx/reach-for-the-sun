using UnityEngine;

public class AbilityFlap : Ability
{
    //hardcoded values for now
    private float _flapForce = 10f;
    private float _flapCooldown = 5f;
    
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;

    public override void Init()
    {
        base.Init();
        AbilityName = "Flap";
        AbilityCooldownDuration = _flapCooldown;
        AbilityEffectDuration = 0;

        _rb = _player.GetComponent<Rigidbody>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
    }

    public override void Effect(bool doCooldown)
    {
        _playerMovement.SetPlayerVelocity(new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z));
        _playerMovement.AddForceToPlayer(Vector3.up * _flapForce, ForceMode.Impulse);
        base.Effect(doCooldown);
    }
}
