using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueWriter : MonoBehaviour, IDialogueWriter
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerNameText;

    private bool _skipRequested;

    public IEnumerator WriteText(CharacterData speaker, string text, float typingSpeed)
    {
        _skipRequested = false;
        dialogueText.text = string.Empty;

        // Set speaker name and color
        if (speakerNameText != null && speaker != null)
        {
            speakerNameText.text = speaker.characterName;
            
        }

        // Set text color
        if (speaker != null)
            dialogueText.color = speaker.textColor;

        // Typewriter
        foreach (char c in text)
        {
            if (_skipRequested)
            {
                dialogueText.text = text;
                yield break;
            }

            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        //Debug.Log("Finished writing dialogue text");
    }

    public void Skip() => _skipRequested = true;
}