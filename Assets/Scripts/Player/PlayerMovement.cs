using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //doing gravity calculations myself because unity won't do it for me
    //make sure defaultgravity stays negative
    private float _defaultGravity = -9.81f; //picked arbitrarily based on real world physics. whatever
    private float _gravityScaleFactor = 1f;
    private float _defaultGravityScaleFactor = 1f;

    private float _terminalDirectionalVelocity = 25f;
    private float _terminalDownwardsVelocity = -75f;

    private float _horizontalVelocityMagnitude = 0;
    private float _verticalVelocity = 0;

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
            float targetYVelocity = _rb.linearVelocity.y * Math.Abs(Mathf.Clamp(Camera.main.transform.forward.y, -1, 0));
            _verticalVelocity = targetYVelocity;
        }
    }

    private void FixedUpdate()
    {
        Vector3 glideDirection = new Vector3(_movementDirection.x, 0, _movementDirection.y);

        //most gliding logic is done based on where the camera is pointing. As far as gravity is concerned, facing up and facing down have the same glide properties (0).
        float cameraAbsYVal = Math.Abs(Camera.main.transform.forward.y);

        float pitchInDeg = Camera.main.transform.eulerAngles.x % 360;
        float pitchInRads = Camera.main.transform.eulerAngles.x * Mathf.Deg2Rad;
        float sinPitch = -Mathf.Sin(pitchInRads);
        float cosPitch = Mathf.Cos(pitchInRads);

        //only do these calculations if the player is gliding
        if (_movementDirection != Vector2.zero)
        {
            glideDirection = Camera.main.transform.TransformDirection(glideDirection.normalized);
            if(_horizontalVelocityMagnitude < _terminalDirectionalVelocity)
            {
                //horizontal velocity needs some amount added to it
                //vertical velocity needs that same amount taken away from it.
                
            }
            _horizontalVelocityMagnitude = _rb.linearVelocity.magnitude * Math.Abs(cosPitch);
            _verticalVelocity = _rb.linearVelocity.magnitude * sinPitch;
            Debug.Log("Vert: " + _verticalVelocity + ", Hor: " + _horizontalVelocityMagnitude);
            _gravityScaleFactor = Mathf.Clamp(_defaultGravityScaleFactor * Math.Abs(sinPitch), 0.1f, 1f);
        }
        else
        {
            //we need to put the player in free fall if they're not gliding.
            _gravityScaleFactor = _defaultGravityScaleFactor;
        }

        if (_doGravity) // ">" because downards velocity is negative
        {
            if (_rb.linearVelocity.y > _terminalDownwardsVelocity)
            {
                
            }
            Vector3 gravity = _defaultGravity * _gravityScaleFactor * Time.deltaTime * Vector3.up;
            _verticalVelocity += gravity.y;
            //Debug.Log("Gravity: " + gravity.y);
            Vector3 temp = -_defaultGravity * Mathf.Abs(cosPitch) * Time.deltaTime * new Vector3(1, 0, 1);
            _horizontalVelocityMagnitude += temp.magnitude;
        }

        if (_turnPlayer)
        {
            //find a point directly ahead of the player
            Vector3 lookDirection = transform.position + new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y * _yWeight, _rb.linearVelocity.z);
            transform.LookAt(lookDirection);
        }

        if(_doMovement)
        {
            _rb.linearVelocity = new Vector3(glideDirection.x * _horizontalVelocityMagnitude, _verticalVelocity, glideDirection.z * _horizontalVelocityMagnitude);
        }
        //Debug.Log(_rb.linearVelocity.magnitude);
    }

    public void SetPlayerVelocity(Vector3 velocity)
    {
        _horizontalVelocityMagnitude = new Vector2(velocity.x, velocity.z).magnitude;
        _verticalVelocity = velocity.y;
    }

    /// <summary>
    /// This method needs to exist because the way I'm doing physics involves constantly overriding the linear velocity of the player.
    /// Adding a force won't work because the velocity gained from that force will get immediately overwritten.
    /// </summary>
    /// <param name="force">A Vector3 representing the force to add</param>
    /// <param name="forceMode">The mode of force to add</param>
    public void AddForceToPlayer(Vector3 force, ForceMode forceMode)
    {
        _rb.AddForce(force, forceMode);
        _horizontalVelocityMagnitude = new Vector2(force.x, force.z).magnitude;
        _verticalVelocity = force.y;
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
            _verticalVelocity = 0;
        }
        _doMovement = moveState;
    }

    public void SetTurnPlayer(bool turnPlayer)
    {
        _turnPlayer = turnPlayer;
    }
}
