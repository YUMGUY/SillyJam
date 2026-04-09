using UnityEngine;

public interface IDialogueContext 
{
    IDialogueWriter Writer { get; }
    IDialogueUIController UI { get; }
    ICharacterSpriteController SpriteController { get; }
    StrikeSystem StrikeSystem { get; }
    int LastChosenIndex { get; set; } // replace with Dialogue Choice
    bool LastChoiceWasCorrect { get; set; }
    //DialogueChoice lastPickedChoice

    IAudioService AudioService { get; }
}