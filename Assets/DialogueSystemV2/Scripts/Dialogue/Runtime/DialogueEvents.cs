using System;
using UnityEngine;
public static class DialogueEvents
{
    public static event Action OnDialogueEnded;
    public static event Action<DialogueEndResult> OnClosingDialogueEnded;
    public static void DialogueEnded() => OnDialogueEnded?.Invoke();
    public static void ClosingDialogueEnded(DialogueEndResult result) => OnClosingDialogueEnded?.Invoke(result);
}