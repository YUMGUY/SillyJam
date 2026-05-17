using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SubtitleAsset : PlayableAsset, ITimelineClipAsset
{
    [TextArea(3, 10)]
    public string subtitleText;

    [Tooltip("Characters per second")]
    public float typingSpeed = 20f;

    [Tooltip("Delay in seconds before typing starts within this clip")]
    public float startDelay = 0f;

    public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.Extrapolation;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SubtitleBehavior>.Create(graph);
        SubtitleBehavior behaviour = playable.GetBehaviour();

        behaviour.subtitleText = subtitleText;
        behaviour.typingSpeed = Mathf.Max(0.1f, typingSpeed);
        behaviour.startDelay = Mathf.Max(0f, startDelay);

        return playable;
    }
}
