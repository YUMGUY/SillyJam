using UnityEngine;

[CreateAssetMenu(fileName = "Choice", menuName = "Scriptable Objects/Choice")]
public class Choice : ScriptableObject
{
    public string choiceText;
    public DialogueBox pathToTake;
    public bool isCorrectChoice; // Must vet the choice array FUTURE
}
