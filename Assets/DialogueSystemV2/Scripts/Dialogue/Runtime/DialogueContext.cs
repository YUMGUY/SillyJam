using UnityEngine;

public class DialogueContext : MonoBehaviour, IDialogueContext
{
    public IDialogueWriter Writer { get; private set; }
    public IDialogueUIController UI { get; private set; }

    public int LastChosenIndex { get; set; } = -1;
    private void Awake()
    {
        Writer = GetComponentInChildren<DialogueWriter>();
        UI = GetComponentInChildren<DialogueUIController>();
        if (Writer == null)
        {
            Debug.LogWarning("<color=orange><b>[DialogueSystem]</b></color> Writer component <color=red>NOT FOUND</color> on children!");
        }
    }
}