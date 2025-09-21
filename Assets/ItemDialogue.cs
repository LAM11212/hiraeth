using UnityEngine;

[CreateAssetMenu(fileName = "ItemDialogue", menuName = "Scriptable Objects/ItemDialogue")]
public class ItemDialogue : ScriptableObject
{
    public string itemName;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    public bool isEndDialogue;
}
