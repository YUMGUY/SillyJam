using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Dialogue/Character Data", fileName = "CharacterData_")]
public class CharacterData : ScriptableObject
{
    [Header("Identity")]
    public string characterName;

    [Header("Visuals")]
    public Image textPlate;
    public Image defaultCharacterSprite;
    public Image nameSourceImage;
    public Color textColor;
    

    [Header("Audio")]
    public AudioClip voiceBlip; // the per-character typing sound
}