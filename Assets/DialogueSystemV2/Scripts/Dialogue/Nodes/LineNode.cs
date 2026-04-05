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
            Debug.Log("Dialogue finished at: " + name);
            yield break;
        }

        // 3. Wait for player to press advance
        // yield return ctx.UI.WaitForAdvance();

        // Auto advance after delay
        yield return new WaitForSeconds(autoAdvanceDelay);

        // 4. Fire post-commands
        foreach (var cmd in postCommands)
            yield return cmd.Execute(ctx);
    }

    public override DialogueNode GetNext(IDialogueContext ctx) => nextNode;
}