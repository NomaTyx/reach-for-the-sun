using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // initial cursor state
    [field: SerializeField] protected CursorLockMode CursorMode { get; set; } = CursorLockMode.Locked;
    // make character look in Camera direction instead of MoveDirection
    [field: SerializeField] protected bool LookInCameraDirection { get; set; }
    [field: SerializeField] private float DashForce = 100;
    [field: SerializeField] private float LaunchForce = 100;
    [SerializeField] private Transform cameraTransform;

    [field: Header("Componenents")]

    [SerializeField] private bool _turnPlayer = true;
    [SerializeField] private float _yWeight = 3;

    protected Vector2 MoveInput { get; set; }

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnLaunchPlayerUpwards()
    {
        Debug.Log("Added Force.");
        rb.AddForce(new Vector3(10, LaunchForce, 10), ForceMode.Impulse);
    }

    public void OnDash()
    {
        Vector3 playerVelocity = rb.linearVelocity;
        rb.linearVelocity = Vector3.zero;

        Vector3 newDirection = new Vector3(cameraTransform.forward.x, rb.linearVelocity.y, cameraTransform.forward.z);

        //possibly change this to just set the velocity equal to the value, who knows
        rb.AddForce(newDirection * DashForce, ForceMode.Impulse);
        transform.LookAt(transform.position + newDirection);
    }

    protected virtual void Update()
    {
        if (_turnPlayer)
        {
            transform.LookAt(transform.position + new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * _yWeight, rb.linearVelocity.z));
        }
    }
}
