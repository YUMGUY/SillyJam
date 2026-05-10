using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[System.Serializable]
public class MultiImageFadeClip : PlayableAsset, ITimelineClipAsset
{
    public ExposedReference<Image> imageToFade;
    public MultiImageFadeBehavior template = new MultiImageFadeBehavior();

    // Tells Timeline if the clips can blend/overlap
    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MultiImageFadeBehavior>.Create(graph, template);
        var clone = playable.GetBehaviour();

        // RESOLVE: This connects the "Hook" to the actual Scene Image
        clone.targetImage = imageToFade.Resolve(graph.GetResolver());

        return playable;
    }
}