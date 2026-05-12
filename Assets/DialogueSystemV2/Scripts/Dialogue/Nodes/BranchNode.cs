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
        if (ctx.LastPickedChoice != null)
            return ctx.LastPickedChoice.nextNode;

        Debug.LogWarning("BranchNode: No valid choice picked set on context");
        return null;
    }
}