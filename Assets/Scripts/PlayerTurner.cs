using UnityEngine;
using UnityEngine.Timeline;

public class PlayerTurner : MonoBehaviour
{
    //this value will be disabled in certain cases
    public bool TurnPlayer = true;

    [SerializeField] private int _rotationSpeed = 5;
    [SerializeField] private int _yWeight = 3;

    private Rigidbody rb;
    private Quaternion rotation;
    float time = 0;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    // rotates the thing to face the correct direction every frame yippeeeeeeeeeee we love doing things in update!
    void Update()
    {
        if(TurnPlayer)
        {
            transform.LookAt(transform.position + new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * _yWeight, rb.linearVelocity.z));
        }
    }
}
