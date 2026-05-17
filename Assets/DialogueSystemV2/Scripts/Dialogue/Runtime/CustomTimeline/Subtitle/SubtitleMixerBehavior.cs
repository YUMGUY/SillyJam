using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleMixerBehavior : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI targetText = playerData as TextMeshProUGUI;
        if (targetText == null) return;

        int inputCount = playable.GetInputCount();

        string finalText = "";
        float totalWeight = 0f;
        float highestWeight = -1f;

        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);
            if (weight <= 0f) continue;

            ScriptPlayable<SubtitleBehavior> inputPlayable =
                (ScriptPlayable<SubtitleBehavior>)playable.GetInput(i);
            SubtitleBehavior behaviour = inputPlayable.GetBehaviour();

            double clipTime = inputPlayable.GetTime();

            // The clip with the highest weight dictates the text content being typed
            if (weight > highestWeight)
            {
                highestWeight = weight;
                finalText = behaviour.GetCurrentText(clipTime);
            }

            totalWeight += weight;
        }

        // Apply text content
        targetText.text = finalText;

        // Apply text alpha based on the accumulated track weight
        Color c = targetText.color;
        c.a = Mathf.Clamp01(totalWeight);
        targetText.color = c;

        // Force TMPro to redraw layout meshes
        targetText.SetVerticesDirty();
    }
}
