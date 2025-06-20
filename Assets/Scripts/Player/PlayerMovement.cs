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
            Debug.Log(_rb.linearVelocity.y);
        }
        if (_doGliding)
        {
            float xVelocity = _movementDirection.x * _glideSpeed;
            float zVelocity = _movementDirection.y * _glideSpeed;
            _rb.linearVelocity = new Vector3(xVelocity, _rb.linearVelocity.y, zVelocity);
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
