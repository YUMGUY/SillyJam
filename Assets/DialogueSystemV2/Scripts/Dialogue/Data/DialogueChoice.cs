using System;
using UnityEngine;

[Serializable]
public class DialogueChoice
{
    [TextArea(2, 2)]
    public string label;
    public DialogueNode nextNode;

    [Header("Strike system")]
    public ChoiceResult choiceResult;
    //public bool isCorrectChoice;
}