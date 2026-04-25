using System;
using UnityEngine;
public static class DialogueEvents
{
    public static event Action OnDialogueEnded;
    public static event Action OnAllGoodStrikesFilled;
    public static event Action OnAllBadStrikesFilled;
    public static void DialogueEnded() => OnDialogueEnded?.Invoke();
    public static void AllGoodStrikesFilled() => OnAllGoodStrikesFilled?.Invoke();
    public static void AllBadStrikesFilled() => OnAllBadStrikesFilled?.Invoke();
}