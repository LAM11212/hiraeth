using System.Collections;
using TMPro;
using UnityEngine;

public class EndInteractableObject : MonoBehaviour, IInteractable
{
    public ItemDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text NameText;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    [SerializeField] GameObject gameendButton;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null) return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        if (NameText != null)
            NameText.SetText(dialogueData.itemName);
        isDialogueActive = true;
        dialogueIndex = 0;

        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());

    }

    private void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);

        CutsceneController cutscene = Object.FindFirstObjectByType<CutsceneController>();
        if (cutscene != null)
        {
            cutscene.EndCutscene();
        }
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void OnYesButtonPressed()
    {
        SceneController.instance.NextLevel();
    }

    public void OnEndButtonPressed()
    {
        if (dialogueData != null && dialogueData.isEndDialogue)
        {
            StartCoroutine(HandleGameEnding());
            return;
        }
    }

    private IEnumerator HandleGameEnding()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        GameObject gun = GameObject.FindGameObjectWithTag("gun");

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (gun != null)
            gun.SetActive(false);

        Animator playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        if (playerAnim != null)
        {
            playerAnim.SetTrigger("EndAnimation");
        }

        yield return new WaitForSeconds(2f); // adjust 3f to whatever the gg animation is.
        Debug.Log("exiting game. . .");
        Application.Quit();
    }

}
