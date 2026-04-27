using System.Collections;
using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField] private DialogueNode entryNode;

    private IDialogueContext _ctx;
    public Coroutine dialogueRunnerWorker;
    private Coroutine closingDialogueRunnerWorker;
    [SerializeField] private DialogueGraph dialogueGraph;
    [SerializeField] private DialogueNode entryClosingDialogue;
    public bool hasEnded = false;
    private void Awake()
    {
        _ctx = GetComponent<IDialogueContext>();
        if(_ctx == null)
        {
            Debug.Log("Missing dialogue context at Awake");
        }
    }

    private void Start()
    {
        if (dialogueGraph != null)
        {
            Debug.Log("Dialogue Runner automatically started from Start");
            StartDialogue(dialogueGraph.entryNode);
        }
        else
        {
            Debug.Log("The entry dialogue graph is missing!");
        }
    }

    public void StartDialogue(DialogueNode entry)
    {
        hasEnded = false;
        dialogueRunnerWorker = StartCoroutine(RunDialogue(entry));
    }

    // Activated when conversation timer runs out or Strikes are filled out (good/bad)
    public void ForceEndDialogue()
    {
        if (hasEnded) return; // guard against double firing

        hasEnded = true;

        if (dialogueRunnerWorker != null)
        {
            StopCoroutine(dialogueRunnerWorker);
            dialogueRunnerWorker = null;
        }


        Debug.Log("Dialogue force ended early by the Conversation Timer or by Strike System");
        _ctx.UI.ForceStop();
        _ctx.AudioService.StopMusic();
        DialogueEvents.DialogueEnded();

        // dialgoue ends, conversation timer and battle box stops => ending dialogue starts => ending dialogue ends and starts respective Timeline
        StartClosingDialogue();
    }


    private IEnumerator RunDialogue(DialogueNode node)
    {
        while (node != null)
        {
            yield return node.Execute(_ctx);
            node = node.GetNext(_ctx);
        }

        if (hasEnded) yield break; // protect against unlikely race condition where forceenddialogue is called exactly at the same frame

        hasEnded = true;
        Debug.Log("<color=green>Dialogue finished overall. Finished naturally!</color>");
        _ctx.AudioService.StopMusic();
        DialogueEvents.DialogueEnded();
        // dialgoue ends naturally (which means Mediocre ending achieved????), conversation timer and battle box stops => ending dialogue starts => ending dialogue ends and starts respective Timeline

        // RunClosingDialogue
        StartClosingDialogue();
    }

    // Handle end
    private void StartClosingDialogue()
    {
        if (entryClosingDialogue == null)
        {
            Debug.LogWarning("No closing dialogue assigned");
            return;
        }

        Debug.Log("Running closing dialogue...");
        _ctx.UI.ShowPlayerDialogueBox();
        closingDialogueRunnerWorker = StartCoroutine(RunClosingDialogue(entryClosingDialogue));
    }
    private IEnumerator RunClosingDialogue(DialogueNode node)
    {
        while (node != null)
        {
            yield return node.Execute(_ctx);
            node = node.GetNext(_ctx);
        }

        Debug.Log("<color=cyan>Closing dialogue finished: waiting 2 seconds...</color>");

        yield return new WaitForSeconds(2f);

        Debug.Log("Now the closing Timeline (cg) will play");
        DialogueEndResult result = _ctx.StrikeSystem.EvaluateEnding();
        DialogueEvents.ClosingDialogueEnded(result);
    }
}