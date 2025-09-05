using Unity.VisualScripting;
using UnityEngine;

public class EnableCutsceneDialogue : MonoBehaviour
{
    BoxCollider2D dialogeHitbox;

    private void Awake()
    {
        dialogeHitbox = GetComponent<BoxCollider2D>();
        dialogeHitbox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogeHitbox.enabled = true;
        }
    }
}
