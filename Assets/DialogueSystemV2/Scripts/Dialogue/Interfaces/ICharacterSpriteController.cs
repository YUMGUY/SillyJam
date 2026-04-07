using UnityEngine;
public interface ICharacterSpriteController
{
    void ShowCharacter(CharacterData character);
    void HideCharacter(CharacterData character);
    void ChangeEmotion(CharacterData character, Sprite emotion);
    void HideAll();
}