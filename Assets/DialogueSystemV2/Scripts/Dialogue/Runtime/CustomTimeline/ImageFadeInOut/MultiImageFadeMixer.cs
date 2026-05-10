using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
public class MultiImageFadeMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<MultiImageFadeBehavior> inputPlayable = (ScriptPlayable<MultiImageFadeBehavior>)playable.GetInput(i);
            MultiImageFadeBehavior behavior = inputPlayable.GetBehaviour();

            if (behavior.targetImage != null)
            {
                // Apply the timeline weight directly to the alpha
                Color c = behavior.targetImage.color;
                c.a = inputWeight;
                behavior.targetImage.color = c;
            }
        }
    }
}
