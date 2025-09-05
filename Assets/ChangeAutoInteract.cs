using Unity.VisualScripting;
using UnityEngine;

public class ChangeAutoInteract : MonoBehaviour
{
    public PlayerMovement pm;
    public GameObject blockerPrefab;
    public Transform blockerSpawnPoint;
    public bool modifySpeed = false;
    private float cutsceneWalkSpeed = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("CutscenePlayer"))
        {
            pm = collision.GetComponent<PlayerMovement>();
            if (pm != null && modifySpeed)
            {
                pm.autoInteract = true;
                pm.moveSpeed = cutsceneWalkSpeed;
                pm.dashDistance = 0f;

                GameObject blocker = Instantiate(blockerPrefab, blockerSpawnPoint.position, Quaternion.identity);
                blocker.SetActive(true);
            }
            else if (pm != null && !modifySpeed)
            {
                pm.autoInteract = true;
                GameObject blocker = Instantiate(blockerPrefab, blockerSpawnPoint.position, Quaternion.identity);
                blocker.SetActive(true);
            }
        }
    }
}
