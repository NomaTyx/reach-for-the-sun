using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // initial cursor state
    [field: SerializeField] protected CursorLockMode CursorMode { get; set; } = CursorLockMode.Locked;
    // make character look in Camera direction instead of MoveDirection
    [field: SerializeField] protected bool LookInCameraDirection { get; set; }

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
        rb.AddForce(new Vector3(10, 100, 10), ForceMode.Impulse);
    }

    protected virtual void Update()
    {
        
    }
}
