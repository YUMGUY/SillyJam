using UnityEngine;

public class DialogueContext : MonoBehaviour, IDialogueContext
{
    public IDialogueWriter Writer { get; private set; }
    public IDialogueUIController UI { get; private set; }

    public ICharacterSpriteController SpriteController { get; private set; }
    public StrikeSystem StrikeSystem { get; private set; }
   
    public IAudioService AudioService { get; private set; }
    public int LastChosenIndex { get; set; } = -1;
    public bool LastChoiceWasCorrect { get; set; } = false;
    private void Awake()
    {
        Writer = GetComponentInChildren<DialogueWriter>();
        UI = GetComponentInChildren<DialogueUIController>();
        SpriteController = GetComponentInChildren<CharacterSpriteController>();
        StrikeSystem = GetComponentInChildren<StrikeSystem>();
        AudioService = GetComponentInChildren<DialogueAudioController>();
        if (Writer == null)
        {
            Debug.LogWarning("<color=orange><b>[DialogueSystem]</b></color> Writer component <color=red>NOT FOUND</color> on children!");
        }
        if(SpriteController == null)
        {
            Debug.LogWarning("<color=orange><b>[DialogueSystem]</b></color> Sprite component <color=red>NOT FOUND</color> on children!");
        }
        if (StrikeSystem == null)
        {
            Debug.LogWarning("<color=orange><b>[DialogueSystem]</b></color> StrikeSystem component <color=red>NOT FOUND</color> on children!");
        }
    }
}