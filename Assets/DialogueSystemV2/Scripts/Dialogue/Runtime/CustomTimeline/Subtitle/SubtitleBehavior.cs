using UnityEngine;
using UnityEngine.Playables;
public class SubtitleBehavior : PlayableBehaviour
{
    public string subtitleText;
    public float typingSpeed;
    public float startDelay;

    public string GetCurrentText(double clipTime)
    {
        if (string.IsNullOrEmpty(subtitleText)) return "";

        // Calculate time allocated for typing after the initial delay
        float typingTime = (float)clipTime - startDelay;
        if (typingTime <= 0f) return "";

        // Calculate how many characters should be revealed based on speed
        int characterCount = Mathf.FloorToInt(typingTime * typingSpeed);
        characterCount = Mathf.Clamp(characterCount, 0, subtitleText.Length);

        return subtitleText.Substring(0, characterCount);
    }
}
