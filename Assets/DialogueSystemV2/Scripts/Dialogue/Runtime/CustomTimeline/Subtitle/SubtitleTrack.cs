using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.2f, 0.6f, 0.9f)]
[TrackClipType(typeof(SubtitleAsset))]
[TrackBindingType(typeof(TextMeshProUGUI))]
public class SubtitleTrack:TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SubtitleMixerBehavior>.Create(graph, inputCount);
    }
}
