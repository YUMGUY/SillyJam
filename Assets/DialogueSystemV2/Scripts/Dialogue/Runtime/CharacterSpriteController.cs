using UnityEngine;
using UnityEngine.UI;

public class CharacterSpriteController : MonoBehaviour, ICharacterSpriteController
{
    [Header("Character Slots")]
    [SerializeField] private Image playerSlot;
    [SerializeField] private Image npcSlot;

    [Header("Characters")]
    [SerializeField] private CharacterData playerCharacter;

    // Called once at conversation start
    public void ShowCharacter(CharacterData character)
    {
        Image slot = GetSlotForCharacter(character);
        if (slot == null) return;

        // Use default sprite when first showing
        slot.sprite = character.defaultSprite;
        slot.gameObject.SetActive(true);
    }

    // Called mid-conversation to swap expression
    public void ChangeEmotion(CharacterData character, Sprite emotion)
    {
        Image slot = GetSlotForCharacter(character);
        if (slot == null) 
        {
            Debug.LogWarning("Could not find character or player.");
            return;
        }

        // Slot stays active, just swap the sprite
        slot.sprite = emotion != null ? emotion : character.defaultSprite;
    }

    // Called at conversation end
    public void HideCharacter(CharacterData character)
    {
        Image slot = GetSlotForCharacter(character);
        if (slot != null)
            slot.gameObject.SetActive(false);
    }

    public void HideAll()
    {
        if (playerSlot != null) playerSlot.gameObject.SetActive(false);
        if (npcSlot != null) npcSlot.gameObject.SetActive(false);
    }

    private Image GetSlotForCharacter(CharacterData character)
    {
        if (character == playerCharacter) // for now assume we only talk to 1 character at a time
            return playerSlot;
        else
            return npcSlot;
    }
}