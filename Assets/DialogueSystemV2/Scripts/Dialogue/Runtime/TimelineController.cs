using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    [Header("Timelines")]
    [SerializeField] private PlayableDirector goodEndingTimeline;
    [SerializeField] private PlayableDirector badEndingTimeline;
    [SerializeField] private PlayableDirector midEndingTimeline;

    private void PlayEndingCG(DialogueEndResult endResult)
    {
        switch(endResult)
        {
            case DialogueEndResult.Good:
                if (goodEndingTimeline != null)
                    goodEndingTimeline.Play();
                break;
            case DialogueEndResult.Bad:
                if (badEndingTimeline != null)
                    badEndingTimeline.Play();
                break;
            case DialogueEndResult.Mediocre:
                if(midEndingTimeline != null)
                    midEndingTimeline.Play();
                break;
        }
    }

    private void OnEnable()
    {
        DialogueEvents.OnClosingDialogueEnded += PlayEndingCG;
    }

    private void OnDisable()
    {
        DialogueEvents.OnClosingDialogueEnded -= PlayEndingCG;
    }

    public void TestBadCloser()
    {
        if (badEndingTimeline != null)
        {
            Debug.Log("Tested bad ending cg");
            badEndingTimeline.Play();
        }
           
    }
}