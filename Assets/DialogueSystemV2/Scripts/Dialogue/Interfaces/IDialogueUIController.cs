using UnityEngine;

using System.Collections;

public interface IDialogueUIController
{
    void ShowDialogueBox();
    void HideDialogueBox();
    void ShowPlayerDialogueBox();
    void HidePlayerDialogueBox();
    void ForceStop();
    IEnumerator ShowChoices(DialogueChoice[] choices);
}