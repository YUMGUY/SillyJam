using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueWriter : MonoBehaviour, IDialogueWriter
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text playerDialogueText;
    [SerializeField] private TMP_Text speakerNameText;

    [Header("Characters")]
    [SerializeField] private CharacterData playerCharacter;

    private bool _skipRequested;

    public IEnumerator WriteText(CharacterData speaker, string text, float typingSpeed)
    {
        _skipRequested = false;

        bool isPlayer = speaker == playerCharacter;
        TMP_Text targetText = isPlayer ? playerDialogueText : dialogueText;

        // 1. Set the full text and color immediately so layout/wrapping is calculated
        targetText.text = text;
        if (speaker != null)
            targetText.color = speaker.textColor;

        // 2. Hide all characters initially
        targetText.maxVisibleCharacters = 0;

        // 3. Force TMP to update the mesh so it knows exactly how many characters there are
        targetText.ForceMeshUpdate();

        int totalCharacters = text.Length;

        // 4. Typewriter reveal using maxVisibleCharacters
        for (int i = 0; i <= totalCharacters; i++)
        {
            if (_skipRequested)
            {
                // Reveal everything immediately
                targetText.maxVisibleCharacters = totalCharacters;
                yield break;
            }

            targetText.maxVisibleCharacters = i;

            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void Skip() => _skipRequested = true;
}