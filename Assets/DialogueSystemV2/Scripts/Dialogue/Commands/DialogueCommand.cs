using System.Collections;
using UnityEngine;

public abstract class DialogueCommand : ScriptableObject
{
    public abstract IEnumerator Execute(IDialogueContext ctx);
}