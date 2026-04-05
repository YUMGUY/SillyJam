using UnityEngine;

using System.Collections;

public interface IDialogueWriter
{
    IEnumerator WriteText(CharacterData speaker, string text, float typingSpeed);
    void Skip();
}