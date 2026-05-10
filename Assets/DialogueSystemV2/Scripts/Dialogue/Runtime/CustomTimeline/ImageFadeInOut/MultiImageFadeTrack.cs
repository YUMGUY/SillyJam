using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.UI;

[TrackColor(0.1f, 0.8f, 0.4f)]
[TrackClipType(typeof(MultiImageFadeClip))]
public class MultiImageFadeTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MultiImageFadeMixer>.Create(graph, inputCount);
    }
}