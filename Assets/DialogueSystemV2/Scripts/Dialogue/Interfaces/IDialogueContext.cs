using UnityEngine;

public interface IDialogueContext 
{
    IDialogueWriter Writer { get; }
    IDialogueUIController UI { get; }
    int LastChosenIndex { get; set; }
}