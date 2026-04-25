using UnityEngine;

using System.Collections;

public interface IDialogueUIController
{
    void ShowDialogueBox();
    void HideDialogueBox();
    void ForceStop();
    IEnumerator ShowChoices(DialogueChoice[] choices);
}