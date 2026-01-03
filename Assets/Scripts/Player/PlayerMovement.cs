using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //doing gravity calculations myself because unity won't do it for me
    //make sure defaultgravity stays negative
    private float _defaultGravity = -9.81f; //picked arbitrarily based on real world physics. whatever
    private float _gravityScaleFactor = 1f;
    private float _defaultGravityScaleFactor = 1f;
    private float _glideGravityScaleFactor = 0.1f;

    private float _terminalDirectionalVelocity = 25f; //unused so far, just adding for posterity
    private float _terminalDownwardsVelocity = -75f;

    [SerializeField] private float _glideSpeed = 20f;
    [SerializeField] private float _glideAccel = 10f;
    [SerializeField] private float _terminalGlideVelocity = 10f;

    private float _horizontalVelocityMagnitude = 0;
    private float _verticalVelocityMagnitude = 0;

    [Header("Player Turning")]
    [SerializeField] private float _yWeight = 3;

    private bool _doGravity = true;
    private bool _doGliding = true;
    private bool _doMovement = true;

    public bool DoGravity => _doGravity;
    public bool DoGliding => _doGliding;
    public bool DoMovement => _doMovement;

    private bool _turnPlayer = true;

    private Vector2 _movementDirection;
    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        if (!_doGliding) return;

        _movementDirection = moveInput;

        if (_movementDirection != Vector2.zero)
        {
            if(_gravityScaleFactor != _glideGravityScaleFactor) 
            {
                float targetYVelocity = _rb.linearVelocity.y * -Mathf.Clamp(Camera.main.transform.forward.y, -1, 0);
                _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, targetYVelocity, _rb.linearVelocity.z);
                _gravityScaleFactor = _glideGravityScaleFactor;
            }
        }
        else
        {
            _gravityScaleFactor = _defaultGravityScaleFactor;
        }
    }

    private void FixedUpdate()
    {
        Vector3 glideDirection = new Vector3(_movementDirection.x, 0, _movementDirection.y);
        

        if (_movementDirection != Vector2.zero)
        {
            glideDirection = Camera.main.transform.TransformDirection(glideDirection.normalized);
            if(Math.Abs(_horizontalVelocityMagnitude) < _terminalDirectionalVelocity)
            {
                _horizontalVelocityMagnitude += _glideAccel;
            }
            _gravityScaleFactor = _glideGravityScaleFactor * 10 * Math.Abs(Mathf.Clamp(Camera.main.transform.forward.y, -1, -0.15f));
        }

        // > because downards velocity is negative
        if (_doGravity && _rb.linearVelocity.y > _terminalDownwardsVelocity)
        {
            Vector3 gravity = _defaultGravity * _gravityScaleFactor * Time.deltaTime * Vector3.up;
            _verticalVelocityMagnitude += gravity.y;
        }

        if (_turnPlayer)
        {
            Vector3 lookDirection = transform.position + new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y * _yWeight, _rb.linearVelocity.z);
            transform.LookAt(lookDirection);
        }

        if(_doMovement)
        {
            _rb.linearVelocity = new Vector3(glideDirection.x * _horizontalVelocityMagnitude, _verticalVelocityMagnitude, glideDirection.z * _horizontalVelocityMagnitude);
        }
        Debug.Log(_verticalVelocityMagnitude);
    }

    public void SetPlayerVelocity(Vector3 velocity)
    {
        _horizontalVelocityMagnitude = new Vector2(velocity.x, velocity.z).magnitude;
        _verticalVelocityMagnitude = velocity.y;
    }

    public void AddForceToPlayer(Vector3 force, ForceMode forceMode)
    {
        _rb.AddForce(force, forceMode);
        _horizontalVelocityMagnitude = new Vector2(force.x, force.z).magnitude;
        _verticalVelocityMagnitude = force.y;
    }

    public void SetGravity(bool gravityState)
    {
        _doGravity = gravityState;
    }
    
    public void SetGliding(bool glideState)
    {
        if (!glideState)
        {
            _movementDirection = Vector2.zero;
        }
        _doGliding = glideState;
    }

    public void SetMovement(bool moveState)
    {
        if (moveState)
        {
            _verticalVelocityMagnitude = 0;
        }
        _doMovement = moveState;
    }

    public void SetTurnPlayer(bool turnPlayer)
    {
        _turnPlayer = turnPlayer;
    }
}
