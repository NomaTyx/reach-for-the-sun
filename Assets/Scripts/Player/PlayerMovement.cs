using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //doing gravity calculations myself because unity won't do it for me
    //make sure defaultgravity stays negative
    private float _defaultGravity = -9.81f; //picked arbitrarily based on real world physics. whatever
    private float _gravityScaleFactor = 1f;

    private float _terminalDirectionalVelocity; //unused so far, just adding for posterity
    private float _terminalDownwardsVelocity = -75f;

    private float _glideSpeed = 5f;

    [Header("Player Turning")]
    [SerializeField] private bool _turnPlayer = true;
    [SerializeField] private float _yWeight = 3;

    private bool _doGravity = true;
    private bool _doGliding = true;
    public bool DoGravity => _doGravity;
    public bool DoMovement => _doGliding;
    
    private Vector2 _movementDirection;
    private Rigidbody _rb;
    private Camera _camera;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GameManager.Instance.Camera;
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        _movementDirection = moveInput;

        if (_movementDirection != Vector2.zero)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _gravityScaleFactor = 0.1f;
        }
        else
        {
            _gravityScaleFactor = 1f;
        }
    }

    private void FixedUpdate()
    {
        if (_doGravity && _rb.linearVelocity.y > _terminalDownwardsVelocity)
        {
            Vector3 gravity = _defaultGravity * _gravityScaleFactor * Vector3.up;
            _rb.AddForce(gravity, ForceMode.Acceleration);
        }

        if (_doGliding && _movementDirection != Vector2.zero)
        {
            Vector3 glideDirection = new Vector3(_movementDirection.x, 0, _movementDirection.y);
            glideDirection = Camera.main.transform.TransformDirection(glideDirection);
            _rb.linearVelocity = glideDirection.normalized * _glideSpeed;
        }

        if (_turnPlayer)
        {
            transform.LookAt(transform.position + new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y * _yWeight, _rb.linearVelocity.z));
        }
    }

    public void ToggleGravity()
    {
        _doGravity = !_doGravity;
    }
    public void ToggleGliding()
    {
        _doGliding = !_doGliding;
    }
}
