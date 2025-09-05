using System.Collections;
using UnityEngine;

public class DisableAutoDialogue : MonoBehaviour
{
    public BoxCollider2D collider;
    public float delay = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DisableAutoInteractBox());
        }
    }

    private IEnumerator DisableAutoInteractBox()
    {
        yield return new WaitForSeconds(delay);
        if (collider != null)
        {
            Debug.Log("collider disabled");
            collider.enabled = false;
        }
    }
}
