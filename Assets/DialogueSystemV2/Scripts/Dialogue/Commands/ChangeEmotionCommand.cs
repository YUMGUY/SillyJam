using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Commands/Change Emotion", fileName = "Cmd_Emotion_")]
public class ChangeEmotionCommand : DialogueCommand
{
    [SerializeField] private CharacterData character;
    [SerializeField] private Sprite emotion;

    public override IEnumerator Execute(IDialogueContext ctx)
    {
        ctx.SpriteController.ChangeEmotion(character, emotion);
        yield return null;
    }
}