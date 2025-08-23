using Unity.VisualScripting;
using UnityEngine;

public class ChangeAutoInteract : MonoBehaviour
{
    private PlayerMovement pm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pm = collision.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.autoInteract = true;
            }
        }
    }
}
