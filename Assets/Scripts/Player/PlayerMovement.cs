using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //doing gravity calculations myself because unity won't do it for me
    //make sure defaultgravity stays negative
    private float _defaultGravity = -9.81f; //picked arbitrarily based on real world physics. whatever
    private float _gravityScaleFactor = 10f;
    private bool _doGravity = true;
    public bool DoGravity => _doGravity;
    
    private Vector2 _movementDirection;
    private Vector3 _zeroYVelocity;
    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _zeroYVelocity = Vector3.zero;
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        _movementDirection = moveInput;

        if (_movementDirection != Vector2.zero)
        {
            _zeroYVelocity.z = _rb.linearVelocity.z;
            _zeroYVelocity.x = _rb.linearVelocity.x; //garbage collection purposes, this is probably cheaper than making a new object every single time

            _rb.linearVelocity = _zeroYVelocity;
            _gravityScaleFactor = 1f;
        }
        else
        {
            _gravityScaleFactor = 10f;
        }
    }

    private void FixedUpdate()
    {
        if (_doGravity)
        {
            Vector3 gravity = _defaultGravity * _gravityScaleFactor * Vector3.up;
            _rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}
