using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    // this is a script specifically made so that i can have an object that moves with the player but doesn't rotate with it.
    private void Update()
    {
        transform.position = playerTransform.position;
    }
}
