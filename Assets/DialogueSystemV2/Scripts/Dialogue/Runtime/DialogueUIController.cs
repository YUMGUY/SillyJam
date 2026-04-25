using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogueUIController : MonoBehaviour, IDialogueUIController
{
    [Header("Dialogue Box")]
    [SerializeField] private GameObject dialogueBox;

    [Header("Choice UI")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Timer")]
    [SerializeField] private float choiceTimeLimit = 5f;
    [SerializeField] private Image timerImage; // optional fill image for countdown

    [Header("Character Data for Reactions")]
    [SerializeField] private CharacterData playerCharacter;
    [SerializeField] private CharacterData npcCharacter;

    [Header("Reaction Sprites")]
    [SerializeField] private Sprite playerThinkSprite;
    [SerializeField] private Sprite playerCorrectSprite;
    [SerializeField] private Sprite playerIncorrectSprite;
    [SerializeField] private Sprite npcCorrectSprite;
    [SerializeField] private Sprite npcIncorrectSprite;

    [Header("Reaction Duration")]
    [SerializeField] private float reactionDuration = 2f;

    // variables to keep track of
    private ChoiceResult _choiceResult;
    private bool _choiceMade;
    private IDialogueContext _ctx;
    private List<GameObject> _spawnedButtons = new List<GameObject>();
    private Coroutine _timerCoroutine;
    private Coroutine _reactionCoroutine;

    private void Awake()
    {
        // DialogueUIController needs context to set LastChosenIndex
        _ctx = GetComponentInParent<IDialogueContext>();
    }

    public void ShowDialogueBox()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(true);
    }

    public void HideDialogueBox()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);
    }

    public IEnumerator ShowChoices(DialogueChoice[] choices)
    {
        _choiceMade = false;
        _choiceResult = ChoiceResult.Skipped; // default to skipped
        choicePanel.SetActive(true);

        SpawnChoiceButtons(choices);
        _timerCoroutine = StartCoroutine(ChoiceTimer(choices));
        _ctx.SpriteController.ChangeEmotion(playerCharacter, playerThinkSprite);

        yield return new WaitUntil(() => _choiceMade); // wait until a choice button is pressed or skipped from the timer running out
        
        if(timerImage!=null) // reset
            timerImage.fillAmount = 1f; 

        // Store result on context
        bool wasCorrect = _choiceResult == ChoiceResult.Correct;
        _ctx.LastChoiceWasCorrect = wasCorrect; // if choice was skipped => false , FIXME to lastchoiceState of correct ,inccorrect, skippped
        _ctx.StrikeSystem.RegisterResult(_choiceResult);

        ClearChoiceButtons();
        choicePanel.SetActive(false);

        // this yield stops the dialogue from continuing , but i can remove it to just play parallel to the dialogue (if parallel, at risk of having choices too close to each other in the future)
        _reactionCoroutine = StartCoroutine(PlayReaction(_choiceResult));
        yield return _reactionCoroutine;
    }

    private IEnumerator ChoiceTimer(DialogueChoice[] choices)
    {
        float elapsed = 0f;
        while (elapsed < choiceTimeLimit)
        {
            elapsed += Time.deltaTime;
            if (timerImage != null)
                timerImage.fillAmount = 1f - (elapsed / choiceTimeLimit);
            yield return null;
        }

        if (!_choiceMade)
        {
            Debug.Log("<color=yellow>Choice timer expired - skipped</color>");
            _choiceResult = ChoiceResult.Skipped;

            // Follow first incorrect path as default, but NO penalty => TODO add isSkippedChoice? boolean to DialogueChoice
            for (int i = 0; i < choices.Length; i++)
            {
                if (!choices[i].isCorrectChoice)
                {
                    // replaced by DialogueChoice todo
                    _ctx.LastChosenIndex = i;
                    break;
                }
            }

            // This means Choice.Skipped is the value used
            _choiceMade = true;
        }
    }

    private IEnumerator PlayReaction(ChoiceResult wasCorrect)
    {
        if (_ctx == null) { Debug.Log("Missing Dialogue Context"); }
        // Swap sprites based on result
        if (wasCorrect == ChoiceResult.Correct)
        {
            _ctx.SpriteController.ChangeEmotion(playerCharacter, playerCorrectSprite);
           // _ctx.SpriteController.ChangeEmotion(npcCharacter, npcCorrectSprite);
        }
        else
        {
            _ctx.SpriteController.ChangeEmotion(playerCharacter, playerIncorrectSprite);
           // _ctx.SpriteController.ChangeEmotion(npcCharacter, npcIncorrectSprite);
        }

        // Hold reaction for duration
        yield return new WaitForSeconds(reactionDuration);

        // Reset back to default sprites
        _ctx.SpriteController.ChangeEmotion(playerCharacter, playerCharacter.defaultSprite);
       // _ctx.SpriteController.ChangeEmotion(npcCharacter, npcCharacter.defaultSprite);
    }


    private void SpawnChoiceButtons(DialogueChoice[] choices)
    {
        ClearChoiceButtons();

        for (int i = 0; i < choices.Length; i++)
        {
            int capturedIndex = i;  // capture for lambda

            GameObject btnObj = Instantiate(choiceButtonPrefab, choicePanel.transform);

            TMP_Text label = btnObj.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = choices[i].label;

            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => OnChoiceSelected(choices[capturedIndex], capturedIndex));

            _spawnedButtons.Add(btnObj);
        }
    }

    private void OnChoiceSelected(DialogueChoice choice, int index)
    {
        if (_choiceMade) return; // guard against double clicks

        // Stop timer
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        //Debug.Log("Chosen choice: " + choice.label); // comment out later
        _choiceResult = choice.isCorrectChoice ? ChoiceResult.Correct : ChoiceResult.Incorrect;

        // replaced by DialogueChoice TODO
        _ctx.LastChosenIndex = index; 
        _choiceMade = true;
    }

    private void ClearChoiceButtons()
    {
        foreach (var btn in _spawnedButtons)
            Destroy(btn);
        _spawnedButtons.Clear();
    }

    public void ForceStop()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
        if (_reactionCoroutine != null)
        {
            StopCoroutine(_reactionCoroutine);
            _reactionCoroutine = null;
        }

        if (_ctx?.SpriteController != null)
        {
            _ctx.SpriteController.ChangeEmotion(playerCharacter, playerCharacter.defaultSprite);
        }
        _choiceMade = true; // unblock WaitUntil if needed
        ClearChoiceButtons();
        choicePanel.SetActive(false);
    }

    private void OnDisable()
    {
        ClearChoiceButtons();
        if(choicePanel != null && choicePanel.activeInHierarchy)
            choicePanel.SetActive(false);
    }

}
public enum ChoiceResult
{
    Correct,
    Incorrect,
    Skipped
}