using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ImageFadeMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Image targetImage = playerData as Image;
        if (targetImage == null) return;

        int inputCount = playable.GetInputCount();
        float totalAlpha = 0f;
        float totalWeight = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);
            if (weight <= 0f) continue;

            ScriptPlayable<ImageFadePlayableBehaviour> inputPlayable =
                (ScriptPlayable<ImageFadePlayableBehaviour>)playable.GetInput(i);

            ImageFadePlayableBehaviour behaviour = inputPlayable.GetBehaviour();

            // Calculate progress normalized to clip duration
            float duration = (float)inputPlayable.GetDuration();
            float time = (float)inputPlayable.GetTime();
            float progress = Mathf.Clamp01(time / duration);

            // Force completion if we are within a tiny margin of the end
            if (duration - time < 0.01f) progress = 1f;

            float alpha = Mathf.Lerp(behaviour.startAlpha, behaviour.endAlpha, progress);
            // Accumulate weighted alpha
            totalAlpha += alpha * weight;
            totalWeight += weight;
        }

        // Apply the final color
        Color c = targetImage.color;

        // If totalWeight is 1.0, totalAlpha is expected.
        // If totalWeight is 0, don't apply anything to avoid glitches.
        if (totalWeight > 0)
        {
            c.a = totalAlpha;
            targetImage.color = c;
        }
    }
}