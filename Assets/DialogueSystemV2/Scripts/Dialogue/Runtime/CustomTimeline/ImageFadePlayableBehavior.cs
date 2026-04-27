using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ImageFadePlayableBehaviour : PlayableBehaviour
{
    public float startAlpha;
    public float endAlpha;

    //public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    //{
    //    Image targetImage = playerData as Image;
    //    if (targetImage == null) return;

    //    float progress = (float)(playable.GetTime() / playable.GetDuration());
    //    float alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

    //    Color c = targetImage.color;
    //    c.a = alpha;
    //    targetImage.color = c;
    //}
}