using System;
using UnityEngine;

[Serializable]
public class DialogueChoice
{
    public string label;
    public DialogueNode nextNode;

    [Header("Strike system")]
    public ChoiceResult choiceResult;
    //public bool isCorrectChoice;
}