using System.Collections;
using UnityEngine;

public class AbilityDash : AbilityBase
{
    private Rigidbody _rb;
    private CapsuleCollider _dashCollider;
    private SphereCollider _bounceCollider;
    private Transform _cameraTransform;

    private bool _isDashing;

    //hardcoded values, these are only stored up here for readability
    private float _dashForce = 100f;
    private float _dashTime = 0.5f;
    private float _dashCooldown = 1f;
    private float _dashHitStopDuration = 0.1f;
    public override void Init()
    {
        base.Init();
        AbilityName = "Dash";

        AbilityEffectDuration = _dashTime;
        AbilityCooldownDuration = _dashCooldown;

        _rb = _player.GetComponent<Rigidbody>();
        _dashCollider = _player.GetComponent<CapsuleCollider>();
        _bounceCollider = _player.GetComponent<SphereCollider>();
        _cameraTransform = GameManager.Instance.Camera.transform;
    }

    public override void Effect(bool doCooldown)
    {
        StartCoroutine(Dash(doCooldown));
    }

    //possibly include logic determining where to target?
    private IEnumerator Dash(bool doCooldown)
    {
        IsActive = true;
        Vector3 playerVelocity = _rb.linearVelocity;
        Vector3 newDirection = _cameraTransform.forward;
        _rb.linearVelocity = Vector3.zero;
        float dashStartTime = Time.time;

        _dashCollider.enabled = true;
        _bounceCollider.enabled = false;

        _isDashing = true;

        _player.GetComponent<PlayerMovement>().ToggleGravity();
        _player.GetComponent<PlayerMovement>().ToggleGliding();

        while (_isDashing)
        {
            _rb.linearVelocity = newDirection * _dashForce;
            if (Time.time >= dashStartTime + AbilityEffectDuration)
            {
                StopDash();
                IsActive = false;
                base.Effect(doCooldown); 
                break;
            }
            yield return null;
        }

        _rb.linearVelocity = new Vector3(playerVelocity.x, 0, playerVelocity.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Health hitHealth = other.GetComponent<Health>();
        if (hitHealth)
        {
            if (IsActive)
            {
                hitHealth.Damage(new DamageInfo(hitHealth.Current, this.gameObject, hitHealth.gameObject));
                TimeManager.Instance.HitStop(_dashHitStopDuration);
                StopDash();
            }
        }
    }

    private void StopDash()
    {
        _isDashing = false;
        _dashCollider.enabled = false;
        _bounceCollider.enabled = true;
        _player.GetComponent<PlayerMovement>().ToggleGravity();
        _player.GetComponent<PlayerMovement>().ToggleGliding();
    }
}
