using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConversationTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueRunner dialogueRunner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static ConversationTimer Instance { get; private set; }

    [Header("Timer Settings")]
    [SerializeField] private float duration = 10f; // Based of of the NPC song of the scene

    [Header("UI")]
    [SerializeField] private Image fillBar;
    [SerializeField] private GameObject timerContainer;

    public bool IsRunning { get; private set; }

    [SerializeField] private float _elapsed;
    private Coroutine _timerCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Public API

    public void StartTimer(float overrideDuration = -1f)
    {
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        // Use override if provided, otherwise use Inspector value
        float timerDuration = overrideDuration > 0f ? overrideDuration : duration;

        _elapsed = 0f;
        _timerCoroutine = StartCoroutine(RunTimer(timerDuration));
    }

    public void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        IsRunning = false;
        HideTimer();
    }

    public void ReduceTime(float amount)
    {
        if (!IsRunning) return;

        _elapsed += amount;
        Debug.Log("Timer reduced by " + amount + "s");
    }

    public void AddTime(float amount)
    {
        if (!IsRunning) return;

        _elapsed -= amount;
        _elapsed = Mathf.Max(0f, _elapsed); // cant go below 0
        Debug.Log("Timer increased by " + amount + "s");
    }

    // Coroutine

    private IEnumerator RunTimer(float timerDuration)
    {
        IsRunning = true;
        _elapsed = 0f;

        if (timerContainer != null)
            timerContainer.SetActive(true);

        while (_elapsed < timerDuration)
        {
            _elapsed += Time.deltaTime;

            if (fillBar != null)
                fillBar.fillAmount = 1f - (_elapsed / timerDuration);

            yield return null;
        }

        // Timer ran out
        fillBar.fillAmount = 0f;
        IsRunning = false;
        //HideTimer();

        OnTimerExpired();
    }

    // Timer Expired

    private void OnTimerExpired()
    {
        // end the dialogue regardless

        Debug.Log("Conversation timer expired");
        if(dialogueRunner != null && dialogueRunner.isActiveAndEnabled)
            dialogueRunner.ForceEndDialogue();
    }

    // Helpers

    private void HideTimer()
    {
        if (timerContainer != null)
            timerContainer.SetActive(false);

        if (fillBar != null)
            fillBar.fillAmount = 1f; // reset fill for next use
    }

    // testing

    public void EndDialogueTest()
    {
        OnTimerExpired();
    }

    public void RunTest()
    {
        StartCoroutine(RunTimer(2.5f)); // hardcoded but works
    }
}
