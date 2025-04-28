using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnLaunchPlayerUpwards()
    {
        Debug.Log("Added Force.");
        rb.AddForce(new Vector3(10, 1000, 10), ForceMode.Impulse);
    }
}
