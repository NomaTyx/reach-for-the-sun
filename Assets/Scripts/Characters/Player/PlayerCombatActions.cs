using System;
using System.Collections;
using UnityEngine;

public class PlayerCombatActions : MonoBehaviour
{
    [Header("Bouncing Off Enemies")]
    [SerializeField] private float _bounceRange = 10;
    [SerializeField] private float _bounceForce = 100;
    [SerializeField] private float _bounceCooldown = 5f;

    [Header("Dashing")]
    [SerializeField] private float _dashTime = 2.5f;
    [SerializeField] private float _dashForce = 100;
    [SerializeField] private float _dashCooldown = 2f;

    [Header("Parrying")]
    [SerializeField] private float _parryRange = 5f;
    [SerializeField] private float _parryCooldown = 1f;

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
    private float _nextDashTime = 0;
    private float _nextParryTime = 0;
    private float _nextBounceTime = 0;

    //events
    public event Action<float> OnParry;
    public event Action OnDashStarted;
    public event Action<float> OnDashFinished;
    public event Action<float> OnBounce;
    public event Action OnPlayerDeath;

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

    /// <summary>
    /// look for every enemy within bouncing range and deal damage to it. also propel with additional force per enemy
    /// </summary>
    //TODO: make this only check the enemy layer
    public void Bounce()
    {
        if (Time.time < _nextBounceTime) return;

        if (_isDashing) return;

        //StopDash();

        //give extra force for each enemy i guess? idk this is mostly an idea i can do later
        float numOfEnemies = 0;

        foreach (Collider c in Physics.OverlapSphere(transform.position, 10))
        {
            Health hitHealth = c.GetComponent<Health>();
            if (hitHealth == null) continue;
            hitHealth.Damage(new DamageInfo(hitHealth.Current, this.gameObject, hitHealth.gameObject));

            //currently unused variable that tracks how many enemies are being bounced off of.
            numOfEnemies++;
        }

        //later on i will factor in the player's x and y velocity
        Vector3 prevVelocity = _rb.linearVelocity;
        _rb.linearVelocity = Vector3.zero;

        //add more complex logic potentially
        _rb.AddForce(Vector3.up * (_bounceForce + numOfEnemies), ForceMode.Impulse);
        TimeManager.Instance.BulletTime(1);

        OnBounce?.Invoke(_bounceCooldown);

        _nextBounceTime = Time.time + _bounceCooldown;
    }

    //todo: make dash FASTER than normal movement!
    public IEnumerator Dash()
    {
        if (Time.time < _nextDashTime) yield break;

        //cooldown has to account for the time the player spends dashing
        _nextDashTime = Time.time + _dashCooldown + _dashTime;

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

        OnDashFinished?.Invoke(_dashCooldown);
    }

    /// <summary>
    /// stops the dash on the spot, re-enabling all relevant colliders
    /// </summary>
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
        if (Time.time < _nextParryTime) return;

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
        OnParry?.Invoke(_parryCooldown);

        _nextParryTime = Time.time + _parryCooldown;
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
        Debug.Log("player took damage");
    }

    private void DeathBehavior(GameObject deadObj)
    {
        GetComponentInChildren<Shatterer>(true).gameObject.SetActive(true);
        OnPlayerDeath?.Invoke();
    }

    
}
