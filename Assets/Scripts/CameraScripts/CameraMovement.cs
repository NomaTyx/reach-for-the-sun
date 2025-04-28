using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private CinemachineFreeLook cam;

    private void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }

    void FixedUpdate()
    {
        Debug.Log(cam.GetRig(0));
        Debug.Log(cam.GetRig(1));
        Debug.Log(cam.GetRig(2));
        Debug.Log(cam.GetRig(3));
    }
}
