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
    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        _movementDirection = moveInput;

        if (_movementDirection != Vector2.zero)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
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
