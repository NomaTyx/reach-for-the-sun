using System.Collections;
using UnityEngine;

public class AbilityDash : AbilityBase
{
    private Rigidbody _rb;
    private CapsuleCollider _dashCollider;
    private SphereCollider _bounceCollider;

    //hardcoded values
    private float _dashForce = 100f;

    public override void Init()
    {
        _rb = _player.GetComponent<Rigidbody>();
        _dashCollider = _player.GetComponent<CapsuleCollider>();
        _bounceCollider = _player.GetComponent<SphereCollider>();
    }

    public override void Effect(bool doCooldown)
    {
        StartCoroutine(Dash());
    }

    //possibly include logic determining where to target?
    private IEnumerator Dash()
    {
        Vector3 playerVelocity = _rb.linearVelocity;
        Vector3 newDirection = _cameraTransform.forward;
        _rb.linearVelocity = Vector3.zero;
        float dashStartTime = Time.time;

        _dashCollider.enabled = true;
        _bounceCollider.enabled = false;

        _rb.useGravity = false;

        while (_isDashing)
        {
            _rb.linearVelocity = newDirection * _dashForce;
            if (Time.time >= dashStartTime + EffectDuration)
            {
                StopDash();
            }
            yield return null;
        }

        _rb.linearVelocity = playerVelocity;

        base.Effect(doCooldown);
    }

    private void StopDash()
    {
        _isDashing = false;
        _rb.useGravity = true;
        _dashCollider.enabled = false;
        _bounceCollider.enabled = true;
    }
}
