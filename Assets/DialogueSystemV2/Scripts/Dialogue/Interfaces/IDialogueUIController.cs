using UnityEngine;

using System.Collections;

public interface IDialogueUIController
{
    void ShowDialogueBox();
    void HideDialogueBox();

    IEnumerator ShowChoices(DialogueChoice[] choices);
}