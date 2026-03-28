using UnityEngine;

[CreateAssetMenu(fileName = "Speaker", menuName = "Scriptable Objects/Speaker")]
public class Speaker : ScriptableObject
{
    public string speakerName;
    public Color nameColor;
    public int numLabel;
}
