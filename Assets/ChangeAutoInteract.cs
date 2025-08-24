using Unity.VisualScripting;
using UnityEngine;

public class ChangeAutoInteract : MonoBehaviour
{
    private PlayerMovement pm;
    public GameObject blockerPrefab;
    public Transform blockerSpawnPoint;
    private float cutsceneWalkSpeed = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pm = collision.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.autoInteract = true;
                pm.moveSpeed = cutsceneWalkSpeed;
                pm.dashDistance = 0f;

                GameObject blocker = Instantiate(blockerPrefab, blockerSpawnPoint.position, Quaternion.identity);
                blocker.SetActive(true);
            }
        }
    }
}
