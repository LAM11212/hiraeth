using Unity.Cinemachine;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public CinemachineCamera virtualCam; 
    public int activePriority = 20;
    public int inactivePriority = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.Priority = activePriority;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.Priority = inactivePriority;
        }
    }
}
