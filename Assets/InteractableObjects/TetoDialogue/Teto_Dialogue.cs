using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Teto_Dialogue : MonoBehaviour
{
    [SerializeField] private BoxCollider2D dialogueTrigger;
    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private Button yesButton;

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
                StartCoroutine(RedisplayDialogue());

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
    }
}
