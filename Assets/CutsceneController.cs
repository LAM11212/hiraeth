using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public Animator animator;
    public InteractableObject dialogueObject;

    public void StartCutscene()
    {
        PlayerMovement.Instance.SetState(PlayerState.Cutscene);
        animator.SetTrigger("PlayCutscene");

        if (dialogueObject != null)
        {
            dialogueObject.Interact();
        }
    }

    public void EndCutscene()
    {
        PlayerMovement.Instance.SetState(PlayerState.Normal);
    }
}
