using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogueUIController : MonoBehaviour, IDialogueUIController
{
    [Header("Dialogue Boxes")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject playerDialogueBox;

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

    public void ShowPlayerDialogueBox()
    {
        if (playerDialogueBox != null)
            playerDialogueBox.SetActive(true);
    }

    public void HidePlayerDialogueBox()
    {
        if (playerDialogueBox != null)
            playerDialogueBox.SetActive(false);
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
        
        if(timerImage!=null)
            timerImage.fillAmount = 1f; 

        // Store result on context
        _ctx.StrikeSystem.RegisterResult(_choiceResult);
        if (_ctx.LastPickedChoice.nextNode is LineNode lineNode && lineNode.IsEndNode)
        {
            if (_ctx.LastPickedChoice.choiceResult == ChoiceResult.Skipped)
                Debug.Log("Stopped timer even when skipped choice bc endnode is next.");
            ConversationTimer.Instance.StopTimer();
        }
        ClearChoiceButtons();
        choicePanel.SetActive(false);

        // TODO  timer still goes down durimg play reaction, maybe no yield return, needs testing
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

            // Follow Skipped path
            for (int i = 0; i < choices.Length; i++)
            {
                if (choices[i].choiceResult == ChoiceResult.Skipped)
                {
                    _ctx.LastPickedChoice = choices[i];
                    break;
                }
            }

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
            _ctx.SpriteController.ChangeEmotion(npcCharacter, npcCorrectSprite);
        }
        else
        {
            _ctx.SpriteController.ChangeEmotion(playerCharacter, playerIncorrectSprite);
            _ctx.SpriteController.ChangeEmotion(npcCharacter, npcIncorrectSprite);
        }

        // Hold reaction for duration
        yield return new WaitForSeconds(reactionDuration);

        // Reset player back to default sprite, npc can stay
        _ctx.SpriteController.ChangeEmotion(playerCharacter, playerCharacter.defaultSprite);
       // _ctx.SpriteController.ChangeEmotion(npcCharacter, npcCharacter.defaultSprite);
    }


    private void SpawnChoiceButtons(DialogueChoice[] choices)
    {
        ClearChoiceButtons();

        for (int i = 0; i < choices.Length; i++)
        {
            int capturedIndex = i;  // capture for lambda

            if (choices[capturedIndex].choiceResult == ChoiceResult.Skipped) // the skipped choice should be last, but this is a precaution
                continue;

            GameObject btnObj = Instantiate(choiceButtonPrefab, choicePanel.transform);

            TMP_Text label = btnObj.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = choices[capturedIndex].label;

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

        _ctx.LastPickedChoice = choice;
        _choiceResult = choice.choiceResult;
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