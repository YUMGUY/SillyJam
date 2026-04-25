using System.Collections;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/Commands/Start Timer", fileName = "Cmd_Start_Timer")]
public class StartTimerCommand : DialogueCommand
{
    public override IEnumerator Execute(IDialogueContext ctx)
    {
        ConversationTimer.Instance.StartTimer();
        yield return null;
    }
}
