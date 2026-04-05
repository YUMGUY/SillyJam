using System.Collections;
using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField] private DialogueNode entryNode;

    private IDialogueContext _ctx;
    public Coroutine dialogueRunnerWorker;

    [SerializeField] private DialogueGraph dialogueGraph;

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

    private IEnumerator RunDialogue(DialogueNode node)
    {
        while (node != null)
        {
            yield return node.Execute(_ctx);
            node = node.GetNext(_ctx);
        }

        Debug.Log("Dialogue finished overall.");
    }
}