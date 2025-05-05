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
        //possibly change this to just set the velocity equal to the value, who knows
        rb.AddForce(new Vector3(cameraTransform.forward.x, rb.linearVelocity.y, cameraTransform.forward.z) * DashForce, ForceMode.Impulse);
    }

    protected virtual void Update()
    {
        
    }
}
