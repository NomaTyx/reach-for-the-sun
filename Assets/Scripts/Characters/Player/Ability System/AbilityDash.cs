using System.Collections;
using UnityEngine;

public class AbilityDash : AbilityBase
{
    public override void Init(GameObject player)
    {

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

        _isDashing = true;
        _rb.useGravity = false;

        OnDashStarted?.Invoke();

        while (_isDashing)
        {
            _rb.linearVelocity = newDirection * _dashForce;
            if (Time.time >= dashStartTime + _dashTime)
            {
                StopDash();
            }
            yield return null;
        }

        _rb.linearVelocity = playerVelocity;

        base.Effect(doCooldown);
    }
}
