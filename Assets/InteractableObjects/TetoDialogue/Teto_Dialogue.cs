using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Teto_Dialogue : MonoBehaviour
{
    [SerializeField] private BoxCollider2D dialogueTrigger;
    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private Button yesButton;
    private Coroutine redisplayCoroutine;

    private void Start()
    {
        yesButton.gameObject.SetActive(false); // Hide the button initially
        yesButton.enabled = false; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!interactableObject.IsDialogueActive())
            {
                interactableObject.Interact();
                redisplayCoroutine = StartCoroutine(RedisplayDialogue());

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(interactableObject.IsDialogueActive())
            {
                interactableObject.EndDialogue();
            }

            if(redisplayCoroutine != null)
            {
                StopCoroutine(redisplayCoroutine);
                redisplayCoroutine = null;
            }
        }
    }
    private IEnumerator RedisplayDialogue()
    {
        // Wait until the current dialogue finishes
        yield return new WaitUntil(() => !interactableObject.IsDialogueActive());

        // Wait another 5 seconds
        yield return new WaitForSeconds(5f);

        // Start it again
        interactableObject.Interact();
        redisplayCoroutine = null;
    }
}
