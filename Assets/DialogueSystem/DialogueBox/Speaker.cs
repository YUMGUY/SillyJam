using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Speaker", menuName = "Scriptable Objects/Speaker")]
public class Speaker : ScriptableObject
{
    public string speakerName;
    public Color nameColor;
    public int numLabel;
    public Sprite speakerSprite;
    public Image nameImg;
}
