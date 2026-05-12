using UnityEngine;

public interface IDialogueContext
{
    IDialogueWriter Writer { get; }
    IDialogueUIController UI { get; }
    ICharacterSpriteController SpriteController { get; }
    StrikeSystem StrikeSystem { get; }
    DialogueChoice LastPickedChoice {get; set;}
    IAudioService AudioService { get; }
}