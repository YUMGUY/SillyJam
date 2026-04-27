using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[TrackColor(0.9f, 0.5f, 0.1f)]
[TrackClipType(typeof(ImageFadePlayableClip))]
[TrackBindingType(typeof(Image))]
public class ImageFadeTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<ImageFadeMixerBehaviour>.Create(graph, inputCount);
    }
}