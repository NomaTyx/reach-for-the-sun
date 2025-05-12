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
    [field: SerializeField] private float _dashForce = 100;
    [field: SerializeField] private float _launchForce = 100;

    [Header("Bouncing Off Enemies")]
    [SerializeField] private float _bounceRange = 10;
    [SerializeField] private float _bounceForce = 100;
    [SerializeField] private float _bounceBulletTimeModifier = 0.1f;

    [SerializeField] private Transform cameraTransform;

    [field: Header("Componenents")]

    [SerializeField] private bool _turnPlayer = true;
    [SerializeField] private float _yWeight = 3;

    protected Vector2 MoveInput { get; set; }

    private Rigidbody _rb;
    private CinemachineCamera _cam;
    private SphereCollider _collider;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = GetComponent<CinemachineCamera>();
        _collider = GetComponent<SphereCollider>();
        _collider.radius = _bounceRange;
    }

    public void OnLaunchPlayerUpwards()
    {
        Debug.Log("Added Force.");
        _rb.AddForce(new Vector3(10, _launchForce, 10), ForceMode.Impulse);
    }

    public void OnDash()
    {
        Vector3 playerVelocity = _rb.linearVelocity;
        _rb.linearVelocity = Vector3.zero;

        Vector3 newDirection = new Vector3(cameraTransform.forward.x, _rb.linearVelocity.y, cameraTransform.forward.z);

        //possibly change this to just set the velocity equal to the value, who knows
        _rb.AddForce(newDirection * _dashForce, ForceMode.Impulse);
    }

    /// <summary>
    /// look for every enemy within bouncing range and deal damage to it. also propel with additional force per enemy
    /// </summary>
    //TODO: make this only check the enemy layer
    public void OnBounce()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        TimeManager.Instance.BulletTime(_bounceBulletTimeModifier);
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

    /// <summary>
    /// placeholder coroutine for when i end up properly implementing dashing
    /// </summary>
    /// <returns></returns>
    private IEnumerator Dash()
    {
        yield return null;
    }
}
