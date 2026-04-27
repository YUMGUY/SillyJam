using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class ImageFadePlayableClip : PlayableAsset, ITimelineClipAsset
{
    public float startAlpha = 0f;
    public float endAlpha = 1f;

    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ImageFadePlayableBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.startAlpha = startAlpha;
        behaviour.endAlpha = endAlpha;
        // no targetImage here — comes through mixer

        return playable;
    }
}