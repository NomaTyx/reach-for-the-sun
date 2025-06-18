using UnityEngine;

public class PlayerGlide : MonoBehaviour
{
    private Vector2 _movementDirection;

    public void SetMoveInput(Vector2 moveInput)
    {
        _movementDirection = moveInput;
    }

    private void Update()
    {
        Debug.Log(_movementDirection.ToString());
    }
}
