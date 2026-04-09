using System.Collections;
using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField] private DialogueNode entryNode;

    private IDialogueContext _ctx;
    public Coroutine dialogueRunnerWorker;

    [SerializeField] private DialogueGraph dialogueGraph;

    private bool _forceEnded; // changed by ConversationTimer

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
        dialogueRunnerWorker = StartCoroutine(RunDialogue(entry));
    }

    public void ForceEndDialogue()
    {
       // _forceEnded = true;
       // _ctx.Writer.
        if (dialogueRunnerWorker != null)
            StopCoroutine(dialogueRunnerWorker);

        // Stop timer if still running
        //_ctx.ConversationTimer.StopTimer();

        Debug.Log("Dialogue force ended by timer");
        // this is where you'll hook in "stuff happens after" later
        // e.g. onDialogueTimerExpired.Raise()
    }


    private IEnumerator RunDialogue(DialogueNode node)
    {
        while (node != null)
        {
            yield return node.Execute(_ctx);
            node = node.GetNext(_ctx);
        }

        Debug.Log("Dialogue finished overall. Finished naturally!");

        // onDialogueEnded?.Raise // future event channels
    }
}