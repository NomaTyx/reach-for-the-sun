using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _movementDirection;

    public void SetMoveInput(Vector2 moveInput)
    {
        _movementDirection = moveInput;
    }

    private void Update()
    {
        if(_movementDirection != Vector2.zero)
        {
            GetComponent<Rigidbody>().mass = 0.01f;
        }
    }
}
