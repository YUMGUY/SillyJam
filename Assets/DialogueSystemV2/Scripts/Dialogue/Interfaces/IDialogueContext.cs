using UnityEngine;

public interface IDialogueContext 
{
    IDialogueWriter Writer { get; }
    IDialogueUIController UI { get; }
    ICharacterSpriteController SpriteController { get; }
    int LastChosenIndex { get; set; }
}