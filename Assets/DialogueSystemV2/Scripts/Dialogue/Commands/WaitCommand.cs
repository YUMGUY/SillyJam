using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Commands/Wait", fileName = "Cmd_Wait_")]
public class WaitCommand : DialogueCommand
{
    [SerializeField] private float seconds = 0.5f;

    public override IEnumerator Execute(IDialogueContext ctx)
    {
        yield return new WaitForSeconds(seconds);
    }
}