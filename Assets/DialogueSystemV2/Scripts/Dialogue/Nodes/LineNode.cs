using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Nodes/Line Node", fileName = "Line_")]
public class LineNode : DialogueNode
{
    [Header("Speaker")]
    [SerializeField] private CharacterData speaker;

    [Header("Dialogue")]
    [SerializeField, TextArea(3, 6)] private string text;
    [SerializeField] private float typingSpeed = 0.03f;

    [Header("Next Node")]
    [SerializeField] private DialogueNode nextNode;

    [Header("Commands")]
    [SerializeField] private DialogueCommand[] preCommands;
    [SerializeField] private DialogueCommand[] postCommands;

    [Header("Advance Settings")]
    [SerializeField] private float autoAdvanceDelay = 0.5f;
    [SerializeField] private bool isEndNode = false;

    public bool IsEndNode => isEndNode;
    public float AutoAdvanceDelay => autoAdvanceDelay;

    // Public read access for DialogueWriter
    public CharacterData Speaker => speaker;
    public string Text => text;
    public float TypingSpeed => typingSpeed;

    public override IEnumerator Execute(IDialogueContext ctx)
    {
        if (!IsEndNode && nextNode == null)
            Debug.Log("<color=yellow> Line Node: " + this.name + " does not have a Next Node assigned</color>");

        // 1. Fire pre-commands (sprite changes, music, waits)
        foreach (var cmd in preCommands)
            yield return cmd.Execute(ctx);

        // 2. Write the text
        yield return ctx.Writer.WriteText(speaker, text, typingSpeed);

        if (isEndNode)
        {
            foreach (var cmd in postCommands)
                yield return cmd.Execute(ctx);

            // Final node — log immediately, no wait
            Debug.Log("Dialogue finished at End Node: " + name);
            yield break;
        }

        // FOR NOW WE AUTO ADVANCE TO NEXT DIALOGUE
        // 3. Wait for player to press advance
        // yield return ctx.UI.WaitForAdvance();

        // Auto advance after delay
        yield return new WaitForSeconds(autoAdvanceDelay);

        // 4. Fire post-commands
        foreach (var cmd in postCommands)
            yield return cmd.Execute(ctx);

        if(!isEndNode) // safety check
            ctx.StrikeSystem.ShouldStartEndingSequence();
    }

    public override DialogueNode GetNext(IDialogueContext ctx) => nextNode;

#if UNITY_EDITOR
    private void OnValidate()
    {
        VerifyNode();
    }
    private void VerifyNode()
    {
        if (speaker == null)
            Debug.LogWarning("<color=orange>LineNode: " + name + " has no Speaker assigned</color>");

        if (string.IsNullOrEmpty(text))
            Debug.LogWarning("<color=orange>LineNode: " + name + " has no Text assigned</color>");

        if (!isEndNode && nextNode == null)
            Debug.LogWarning("<color=yellow>LineNode: " + name + " is not an end node but has no Next Node assigned</color>");

        if (isEndNode && nextNode != null)
            Debug.LogWarning("<color=yellow>LineNode: " + name + " is marked as end node but Next Node is still assigned</color>");

        if (typingSpeed <= 0f)
            Debug.LogWarning("<color=orange>LineNode: " + name + " has typing speed of 0 or less</color>");
    }
#endif
}