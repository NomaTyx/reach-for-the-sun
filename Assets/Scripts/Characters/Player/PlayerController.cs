using System.Collections;
using System.Collections.Generic;
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

    public Dictionary<string, AbilityBase> Abilities => _abilities;

    //ability stuff
    private Dictionary<string, AbilityBase> _abilities = new Dictionary<string, AbilityBase>(); //using a string as key for testing, it's probably not the most optimal solution.
    private AbilityManager _abilityManager;


    private void Start()
    {
        Cursor.lockState = CursorMode;
        _combatActions = GetComponent<PlayerCombatActions>();
        _movement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody>();

        _abilityManager = GetComponent<AbilityManager>();
        _abilityManager.InitAbilities();
        _abilities = _abilityManager.Abilities;
    }

    //placeholder method
    public void OnLaunchPlayerUpwards()
    {
        _rb.AddForce(new Vector3(10, _launchForce, 10), ForceMode.Impulse);
    }

    public void OnDash()
    {
        _abilities["dash"].TryUse();
    }

    public void OnBounce()
    {
        //_combatActions.Bounce();
        _abilities["bounce"].TryUse();
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
