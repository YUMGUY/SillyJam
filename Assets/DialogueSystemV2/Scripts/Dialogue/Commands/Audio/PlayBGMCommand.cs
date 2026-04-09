using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Commands/Play BGM", fileName = "Cmd_BGM_")]
public class PlayBGMCommand : DialogueCommand
{
    [SerializeField] private AudioClip clip;

    public override IEnumerator Execute(IDialogueContext ctx)
    {
       ctx.AudioService.PlayMusic(clip);
        yield return null;
    }
}