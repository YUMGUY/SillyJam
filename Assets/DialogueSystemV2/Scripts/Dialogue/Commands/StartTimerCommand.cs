using System.Collections;
using UnityEngine;
public class StartTimerCommand : DialogueCommand
{
    public override IEnumerator Execute(IDialogueContext ctx)
    {
        ConversationTimer.Instance.StartTimer();
        yield return null;
    }
}
