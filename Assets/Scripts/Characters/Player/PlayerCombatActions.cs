using System;
using System.Collections;
using UnityEngine;

public class PlayerCombatActions : MonoBehaviour
{
    [Header("Bouncing Off Enemies")]
    [SerializeField] private float _bounceRange = 10;
    [SerializeField] private float _bounceForce = 100;
    [SerializeField] private float _bounceCooldown;

    [Header("Dashing")]
    [SerializeField] private float _dashTime = 2.5f;
    [SerializeField] private float _dashForce = 100;
    [SerializeField] private float _dashCooldown;

    [Header("Parrying")]
    [SerializeField] private float _parryRange = 5f;

    [Header("Hitstop and bullet time")]
    [SerializeField] private float _bounceBulletTimeModifier = 0.1f;
    [SerializeField] private float _dashHitStopDuration = 250f; //milliseconds

    //components
    [SerializeField] private Transform _cameraTransform;
    private SphereCollider _bounceCollider;
    private CapsuleCollider _dashCollider;
    private Rigidbody _rb;
    private Health _health;

    //player state trackers and cooldowns
    private bool _isDashing;

    public event Action OnParry;
    public event Action<float> OnDash;
    public event Action OnBounce;

    private void Start()
    {
        _bounceCollider = GetComponent<SphereCollider>();
        _dashCollider = GetComponent<CapsuleCollider>();
        _health = GetComponent<Health>();

        _bounceCollider.radius = _bounceRange;
        _rb = GetComponent<Rigidbody>();

        _health.OnDamage += DamageBehavior;
        _health.OnDeath += DeathBehavior;
    }

    private void OnDestroy()
    {
        _health.OnDamage -= DamageBehavior;
        _health.OnDeath -= DeathBehavior;
    }

    public void Bounce()
    {
        if (_isDashing)
        {
            Debug.Log("failed bounce");
            return;
        }
        //StopDash();

        //give extra force for each enemy i guess? idk this is mostly an idea i can do later
        float numOfEnemies = 0;

        foreach (Collider c in Physics.OverlapSphere(transform.position, 10))
        {
            Health hitHealth = c.GetComponent<Health>();
            if (hitHealth == null) continue;
            hitHealth.Damage(new DamageInfo(hitHealth.Current, this.gameObject, hitHealth.gameObject));

            numOfEnemies++;
        }

        //later on i will factor in the player's x and y velocity
        Vector3 prevVelocity = _rb.linearVelocity;
        _rb.linearVelocity = Vector3.zero;

        //add more complex logic potentially
        _rb.AddForce(Vector3.up * (_bounceForce + numOfEnemies), ForceMode.Impulse);
        TimeManager.Instance.BulletTime(1);

        OnBounce?.Invoke();
    }

    //todo: make dash FASTER than normal movement!
    public IEnumerator Dash()
    {
        Vector3 playerVelocity = _rb.linearVelocity;
        Vector3 newDirection = _cameraTransform.forward;
        _rb.linearVelocity = Vector3.zero;
        float dashStartTime = Time.time;

        _dashCollider.enabled = true;
        _bounceCollider.enabled = false;

        _isDashing = true;
        _rb.useGravity = false;

        while (_isDashing)
        {
            _rb.linearVelocity = newDirection * _dashForce;
            Debug.Log("dashing");
            if (Time.time >= dashStartTime + _dashTime)
            {
                StopDash();
            }
            yield return null;
        }

        OnDash?.Invoke(_dashCooldown);
    }

    //i need this method because there will be multiple things that stop the dash.
    private void StopDash()
    {
        _isDashing = false;
        _rb.useGravity = true;
        _dashCollider.enabled = false;
        _bounceCollider.enabled = true;
    }

    //TODO: implement cooldown logic
    public void Parry()
    {
        //check only the projectile layer
        Collider[] colliders = Physics.OverlapSphere(transform.position, _parryRange);

        foreach (Collider c in colliders) 
        {
            c.TryGetComponent<Rocket>(out Rocket rocket);

            if(rocket != null)
            {
                rocket.ParryProjectile();
            }
        }

        //cooldown logic goes here
        OnParry?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        Health hitHealth = other.GetComponent<Health>();
        if (hitHealth)
        {
            if (_isDashing)
            {
                hitHealth.Damage(new DamageInfo(hitHealth.Current, this.gameObject, hitHealth.gameObject));
                TimeManager.Instance.HitStop(_dashHitStopDuration);
                //TODO: add hitstop method here
            }
            else
            {
                TimeManager.Instance.BulletTime(_bounceBulletTimeModifier);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Health>(out Health hitHealth)) return;

        TimeManager.Instance.BulletTime(1);
    }

    private void DamageBehavior(GameObject damagedObj)
    {
        Debug.Log("bro took damage");
    }

    private void DeathBehavior(GameObject deadObj)
    {
        Debug.Log("bro died");
    }
}
