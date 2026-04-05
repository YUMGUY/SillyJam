using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Nodes/Branch Node", fileName = "Branch_")]
public class BranchNode : DialogueNode
{
    [Header("Choices")]
    [SerializeField] private DialogueChoice[] choices;

    // Read by DialogueUIController to build the choice buttons
    public DialogueChoice[] Choices => choices;

    public override IEnumerator Execute(IDialogueContext ctx)
    {
        // Show the choices and wait for player to pick one
        yield return ctx.UI.ShowChoices(choices);
    }

    public override DialogueNode GetNext(IDialogueContext ctx)
    {
        int idx = ctx.LastChosenIndex;

        if (idx >= 0 && idx < choices.Length)
            return choices[idx].nextNode;

        Debug.LogWarning("BranchNode: No valid choice index set on context");
        return null;
    }
}