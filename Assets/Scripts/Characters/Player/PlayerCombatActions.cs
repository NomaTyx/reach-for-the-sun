using System.Collections;
using UnityEngine;

public class PlayerCombatActions : MonoBehaviour
{
    [Header("Bouncing Off Enemies")]
    [SerializeField] private float _bounceRange = 10;
    [SerializeField] private float _bounceForce = 100;
    [SerializeField] private float _bounceBulletTimeModifier = 0.1f;
    [SerializeField] private float _bounceCooldown;

    [Header("Dashing")]
    [SerializeField] private float _dashTime = 2.5f;
    [SerializeField] private float _dashForce = 100;
    [SerializeField] private float _dashCooldown;

    [Header("Parrying")]
    [SerializeField] private float _parryRange = 5f;
    [SerializeField] private float _parryWindow = 0.5f; //seconds not frames

    //components
    [SerializeField] private Transform _cameraTransform;
    private SphereCollider _bounceCollider;
    private CapsuleCollider _dashCollider;
    private Rigidbody _rb;
    private Health _health;

    //player state trackers and cooldowns
    private bool _isDashing;

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
        if (_isDashing) return;
        //StopDash();

        //give extra force for each enemy i guess?
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
        TimeManager.Instance.HitStop(250);
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
    }

    //i need this method because there will be multiple things that stop the dash.
    private void StopDash()
    {
        _isDashing = false;
        _rb.useGravity = true;
        _dashCollider.enabled = false;
        _bounceCollider.enabled = true;
    }

    public IEnumerator Parry()
    {
        //TODO: implement!
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDashing)
        {
            Health hitHealth = other.GetComponent<Health>();
            if (hitHealth)
            {
                hitHealth.Damage(new DamageInfo(hitHealth.Current, this.gameObject, hitHealth.gameObject));
            }
            //TODO: add hitstop method here
        }
        else
        {
            TimeManager.Instance.BulletTime(_bounceBulletTimeModifier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
