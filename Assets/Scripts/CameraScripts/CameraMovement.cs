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

    void LerpCamera()
    {
        
    }


}
