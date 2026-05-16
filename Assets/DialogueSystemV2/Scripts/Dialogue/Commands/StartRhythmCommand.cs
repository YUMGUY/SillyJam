using System.Collections;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/Commands/Start Rhythm Game", fileName = "Cmd_Start_RhythmGame")]
public class StartRhythmGame : DialogueCommand
{
    public override IEnumerator Execute(IDialogueContext ctx)
    {
        RhythmMiniGame.Instance.StartSpawning();
        yield return null;
    }
}
