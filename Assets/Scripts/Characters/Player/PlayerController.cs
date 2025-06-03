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

    [field: Header("Componenents")]
    private Transform _cameraTransform;

    [Header("Player Turning")]
    [SerializeField] private bool _turnPlayer = true;
    [SerializeField] private float _yWeight = 3;

    protected Vector2 MoveInput { get; set; }

    //components
    private PlayerCombatActions _combatActions;
    private PlayerMovement _movement;
    private Rigidbody _rb;
    

    private void Start()
    {
        Cursor.lockState = CursorMode;
        _combatActions = GetComponent<PlayerCombatActions>();
        _movement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody>();
    }

    //placeholder method
    public void OnLaunchPlayerUpwards()
    {
        _rb.AddForce(new Vector3(10, _launchForce, 10), ForceMode.Impulse);
    }

    public void OnDash()
    {
        StartCoroutine(_combatActions.Dash());
    }

    /// <summary>
    /// look for every enemy within bouncing range and deal damage to it. also propel with additional force per enemy
    /// </summary>
    //TODO: make this only check the enemy layer
    public void OnBounce()
    {
        _combatActions.Bounce();
    }

    public void OnParry()
    {
        _combatActions.Parry();
    }

    protected virtual void Update()
    {
        if (_turnPlayer)
        {
            transform.LookAt(transform.position + new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y * _yWeight, _rb.linearVelocity.z));
        }
    }
}
