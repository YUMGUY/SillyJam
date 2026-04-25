using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConversationTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueRunner dialogueRunner;

    public static ConversationTimer Instance { get; private set; }

    [Header("Timer Settings")]
    [SerializeField] private float duration = 10f; // Based of of the NPC song of the scene

    [Header("UI")]
    [SerializeField] private Image fillBar;
    [SerializeField] private GameObject timerContainer;

    public bool IsRunning { get; private set; }

    [SerializeField] private float _elapsed;
    private Coroutine _timerCoroutine;

    [Header("Battle UI")]
    [SerializeField] private GameObject battleBox;
    [SerializeField] private GameObject playerHeart;

    public event Action<float> OnMiniGameStarted;
    /*For bettter visual effect later losing time*/
    // [Header("Penalty Settings")]
    //[SerializeField] private float penaltyLerpDuration = 0.3f; // how long the visual lerp takes
    // private Coroutine _penaltyCoroutine;

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

        OnMiniGameStarted?.Invoke(timerDuration);
    }

    public void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        IsRunning = false;
        //HideTimer();
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
        // End the dialogue regardless
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

    // Event Handling
    private void OnEnable()
    {
        DialogueEvents.OnDialogueEnded += HandleDialogueEnded; // TUNE IN
    }

    private void OnDisable()
    {
        DialogueEvents.OnDialogueEnded -= HandleDialogueEnded; // TUNE OUT
    }

    private void HandleDialogueEnded()
    {
       // Debug.Log("running is:" + IsRunning);

        StopTimer();
        if (battleBox != null) battleBox.SetActive(false);
        if (playerHeart != null) playerHeart.SetActive(false);
        Debug.Log("Conversation Timer Actions Executed After Dialogue Ended");

       // HideTimer();
    }


    // Visual Effect

    //private float _displayElapsed; // visual only — fillBar reads this
    //private IEnumerator RunTimer(float timerDuration)
    //{
    //    IsRunning = true;
    //    _elapsed = 0f;
    //    _displayElapsed = 0f;

    //    if (timerContainer != null)
    //        timerContainer.SetActive(true);

    //    while (_elapsed < timerDuration)
    //    {
    //        _elapsed += Time.deltaTime;
    //        // only write displayElapsed if no penalty lerp is running
    //        if (_penaltyCoroutine == null)
    //            _displayElapsed = _elapsed;

    //        if (fillBar != null)
    //            fillBar.fillAmount = 1f - (_displayElapsed / timerDuration);

    //        yield return null;
    //    }

    //    fillBar.fillAmount = 0f;
    //    IsRunning = false;
    //    OnTimerExpired();
    //}
    //public void ApplyTimePenalty(float amount)
    //{
    //    if (!IsRunning) return;

    //    float targetElapsed = Mathf.Min(_elapsed + amount, duration); // clamp so it cant exceed duration

    //    if (_penaltyCoroutine != null)
    //        StopCoroutine(_penaltyCoroutine);

    //    _penaltyCoroutine = StartCoroutine(LerpPenalty(targetElapsed));

    //    Debug.Log($"Time penalty applied: -{amount}s");
    //}

    //private IEnumerator LerpPenalty(float targetElapsed)
    //{
    //    float startDisplay = _displayElapsed;
    //    float t = 0f;

    //    while (t < 1f)
    //    {
    //        t += Time.deltaTime / penaltyLerpDuration;
    //        _displayElapsed = Mathf.Lerp(startDisplay, targetElapsed, t);

    //        if (fillBar != null)
    //            fillBar.fillAmount = 1f - (_displayElapsed / duration);

    //        yield return null;
    //    }

    //    _displayElapsed = targetElapsed;
    //    _penaltyCoroutine = null;
    //    // _elapsed is already ahead — display will catch up naturally next RunTimer frame
    //}


    // Testing

    public void EndDialogueTest()
    {
        OnTimerExpired();
    }

    public void RunTest(float duration)
    {
        StartCoroutine(RunTimer(duration));
    }
}
