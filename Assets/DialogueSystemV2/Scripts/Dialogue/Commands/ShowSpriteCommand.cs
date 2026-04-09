using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Commands/Show Sprite", fileName = "Cmd_ShowSprite_")]
public class ShowSpriteCommand : DialogueCommand
{
    [SerializeField] private CharacterData character;

    public override IEnumerator Execute(IDialogueContext ctx)
    {
        ctx.SpriteController.ShowCharacter(character);
        yield return null;
    }
}