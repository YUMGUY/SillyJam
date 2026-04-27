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

        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);
            if (weight <= 0f) continue;

            ScriptPlayable<ImageFadePlayableBehaviour> inputPlayable =
                (ScriptPlayable<ImageFadePlayableBehaviour>)playable.GetInput(i);

            ImageFadePlayableBehaviour behaviour = inputPlayable.GetBehaviour();

            float progress = (float)(inputPlayable.GetTime() / inputPlayable.GetDuration());
            float alpha = Mathf.Lerp(behaviour.startAlpha, behaviour.endAlpha, progress);

            Color c = targetImage.color;
            c.a = alpha;
            targetImage.color = c;
        }
    }
}