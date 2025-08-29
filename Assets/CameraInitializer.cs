using Unity.Cinemachine;
using UnityEngine;

public class CameraInitializer : MonoBehaviour
{
    public CinemachineCamera room1cam;

    private void Awake()
    {
        room1cam.Priority = 100;
    }
}
