using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // initial cursor state
    [field: SerializeField] protected CursorLockMode CursorMode { get; set; } = CursorLockMode.Locked;
    // make character look in Camera direction instead of MoveDirection
    [field: SerializeField] protected bool LookInCameraDirection { get; set; }
    [field: SerializeField] private float _launchForce = 100;

    [field: Header("Componenents")]
    private Transform _cameraTransform;

    protected Vector2 MoveInput { get; set; }

    //components
    private Rigidbody _rb;

    //ability stuff
    public Dictionary<string, Ability> Abilities => _abilities;
    private Dictionary<string, Ability> _abilities = new Dictionary<string, Ability>(); //using a string as key for testing, it's probably not the most optimal solution.
    private AbilityManager _abilityManager;

    private PlayerMovement _playerMovement;
    private PlayerAnimations _playerAnimations;

    private Health _health;
    public event Action OnPlayerDeath;

    private void Start()
    {
        _health = GetComponent<Health>();
        _abilityManager = GetComponent<AbilityManager>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAnimations = GetComponent<PlayerAnimations>();

        _health.OnDamage += DamageBehavior;
        _health.OnDeath += DeathBehavior;

        Cursor.lockState = CursorMode;
        _rb = GetComponent<Rigidbody>();

        _abilityManager.InitAbilities();
        _abilities = _abilityManager.Abilities;
    }

    public void OnMove(InputValue value)
    {
        _playerMovement.SetMoveInput(value.Get<Vector2>());
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
        _abilities["parry"].TryUse();
    }

    public void OnFlap()
    {
        _abilities["flap"].TryUse();
    }

    private void OnDestroy()
    {
        _health.OnDamage -= DamageBehavior;
        _health.OnDeath -= DeathBehavior;
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

    public void OnTestKey()
    {
        Debug.Log("Test key hit");
        _playerAnimations.StartSpinning();
    }
}
