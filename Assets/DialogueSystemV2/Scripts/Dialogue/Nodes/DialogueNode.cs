using System.Collections;
using UnityEngine;
public abstract class DialogueNode : ScriptableObject
{
    public abstract IEnumerator Execute(IDialogueContext ctx);
    public abstract DialogueNode GetNext(IDialogueContext ctx);
}
