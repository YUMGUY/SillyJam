using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Graph", fileName = "DialogueGraph_")]
public class DialogueGraph : ScriptableObject
{
    [Header("Entry Point")]
    public DialogueNode entryNode;

    [Header("All Nodes In This Conversation")]
    public DialogueNode[] nodes; // not used at Runtime, opnly for Debugging ; Later on will use Editor Graph

#if UNITY_EDITOR
    [TextArea(2, 4)]
    public string developerNotes;
#endif
}