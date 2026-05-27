using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class ManualTimelineSkipper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayableDirector director;

    [Header("Settings")]
    [Tooltip("List of seconds to jump to (e.g., 0.5, 4.0, 8.5)")]
    [SerializeField] private List<float> skipTimes = new();

    void Start()
    {
        if (director == null) director = GetComponent<PlayableDirector>();

        // Ensure the list is sorted so the logic doesn't break
        skipTimes = skipTimes.OrderBy(t => t).ToList();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PerformManualSkip();
        }
    }

    public void PerformManualSkip()
    {
        if (director == null) return;

        double currentTime = director.time;
        float targetTime = -1f;

        // Find the first time in our list that is further ahead than current time
        // Add a 0.1s buffer so multiple clicks don't get stuck on the same timestamp
        foreach (float time in skipTimes)
        {
            if (time > currentTime + 0.1f)
            {
                targetTime = time;
                break;
            }
        }

        if (targetTime >= 0)
        {
            director.time = targetTime;
            director.Evaluate();
            Debug.Log($"Manually skipped to: {targetTime}s");
        }
        else
        {
            // If no more manual points are found, jump to the end
            director.time = director.duration;
            director.Evaluate();
            Debug.Log("No more skip points. Jumped to end.");
        }
    }
}