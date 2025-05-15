using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // initial cursor state
    [field: SerializeField] protected CursorLockMode CursorMode { get; set; } = CursorLockMode.Locked;
    // make character look in Camera direction instead of MoveDirection
    [field: SerializeField] protected bool LookInCameraDirection { get; set; }
    [field: SerializeField] private float _launchForce = 100;

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

    [field: Header("Componenents")]
    private Transform cameraTransform;

    [Header("Player Turning")]
    [SerializeField] private bool _turnPlayer = true;
    [SerializeField] private float _yWeight = 3;

    protected Vector2 MoveInput { get; set; }

    //player state trackers and cooldowns
    private bool _isDashing;

    //components
    private Rigidbody _rb;
    private SphereCollider _bounceCollider;
    private CapsuleCollider _dashCollider;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _bounceCollider = GetComponent<SphereCollider>();
        _dashCollider = GetComponent<CapsuleCollider>();

        _bounceCollider.radius = _bounceRange;

        Cursor.lockState = CursorMode;
    }

    //placeholder method
    public void OnLaunchPlayerUpwards()
    {
        Debug.Log("Added Force.");
        _rb.AddForce(new Vector3(10, _launchForce, 10), ForceMode.Impulse);
    }

    public void OnDash()
    {
        StartCoroutine(Dash());
    }

    /// <summary>
    /// look for every enemy within bouncing range and deal damage to it. also propel with additional force per enemy
    /// </summary>
    //TODO: make this only check the enemy layer
    public void OnBounce()
    {
        if (_isDashing) return;
        //StopDash();

        //give extra force for each enemy i guess?
        float numOfEnemies = 0;

        foreach(Collider c in Physics.OverlapSphere(transform.position, 10))
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

    public void OnParry()
    {
        StartCoroutine(Parry());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDashing)
        {
            Health hitHealth = other.GetComponent<Health>();
            if(hitHealth)
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

    protected virtual void Update()
    {
        if (_turnPlayer)
        {
            transform.LookAt(transform.position + new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y * _yWeight, _rb.linearVelocity.z));
        }
    }

    //todo: make dash FASTER than normal movement!
    private IEnumerator Dash()
    {
        Vector3 playerVelocity = _rb.linearVelocity;
        Vector3 newDirection = cameraTransform.forward;
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
            if(Time.time >= dashStartTime + _dashTime)
            {
                StopDash();
            }
            yield return null;
        }
    }
    
    private IEnumerator Parry()
    {
        //TODO: implement!
        yield return null;
    }

    //i need this method because there will be multiple things that stop the dash.
    private void StopDash()
    {
        _isDashing = false;
        _rb.useGravity = true;
        _dashCollider.enabled = false;
        _bounceCollider.enabled = true;
    }
}
