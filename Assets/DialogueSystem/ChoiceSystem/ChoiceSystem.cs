using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button choiceButtonPrefab;
    [SerializeField] private WriteText dialogueRef;
    [SerializeField] private CharacterAffection character;

    private Choice[] currentChoices;

    private Coroutine choiceTimerCoroutine;
    public TextMeshProUGUI timerText;
    public Image ChoiceOrimage;

    [SerializeField] private float choiceTimeLimit = 5f;

    private Sprite baseCharacterSprite;
    private SpriteRenderer playerSR;
    [SerializeField] private Sprite characterThink;
    [SerializeField] private Sprite playerBadEmote;
    [SerializeField] private Sprite playerGoodEmote;

    // show a timer here
    void Start()
    {
        // Make sure debugging text doesnt show
        timerText.text = "";
        timerText.gameObject.SetActive(false);
        playerSR = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        baseCharacterSprite = playerSR.sprite;
    }

    private void Update()
    {
    }

    // When time runs out, automatically fail the current "stage" of the conversation
    IEnumerator ChoiceTimer()
    {
        playerSR.sprite = characterThink;
        // HERE could be character conversation partner reset back to base pose during choice selection
        int timeRemaining = Mathf.CeilToInt(choiceTimeLimit);
       // timerText.text = timeRemaining.ToString();
        ChoiceOrimage.gameObject.SetActive(true);
        while (timeRemaining > 0)
        {
            //timerText.text = timeRemaining.ToString();

            yield return new WaitForSeconds(1f);

            timeRemaining--;
        }

       // timerText.text = ""; // clear or show "0" if you want

        ChoosePath(null);
    }

    // For now depend on this function for the Conversation Partner Emotion (should be in write text)
    private IEnumerator PlayerAndCharacterReact(bool madeCorrectChoice)
    {
        // our conversation partner
        SpriteRenderer characterSR = character.gameObject.GetComponent<SpriteRenderer>();
        if (!madeCorrectChoice)
        {
            playerSR.sprite = playerBadEmote;
            characterSR.sprite = character.badEmote;
            yield return new WaitForSeconds(2);

            if(choiceTimerCoroutine == null)
            {
                playerSR.sprite = baseCharacterSprite;
                //characterSR.sprite = character.baseEmote;
            }
            characterSR.sprite = character.baseEmote;
        }
        else
        {
            playerSR.sprite = playerGoodEmote;
            characterSR.sprite = character.goodEmote;
            yield return new WaitForSeconds(2);

            if (choiceTimerCoroutine == null)
            {
                playerSR.sprite = baseCharacterSprite;
               // characterSR.sprite = character.baseEmote;
            }
            characterSR.sprite = character.baseEmote;
        }

        yield return null;
    }

    public void DisplayChoices(int numChoices, Choice[] choices)
    {
        currentChoices = choices; // copy
        float startY = -416f;
        float startX = 40;
        for (int i = 0; i < choices.Length; ++i)
        {
            int dindex = i;
            Button buttonMade = Instantiate(choiceButtonPrefab, transform);
            buttonMade.onClick.AddListener(delegate { ChoosePath(choices[dindex]); });
            // position buttons
            buttonMade.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(40 + startX, startY);
            startX += 460;
            // set button text
            buttonMade.GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;
        }

        // Reset timer if already running
        if (choiceTimerCoroutine != null)
            StopCoroutine(choiceTimerCoroutine);

        choiceTimerCoroutine = StartCoroutine(ChoiceTimer());
    }



    public void ChoosePath(Choice chosenPathNode)
    {
        // reset choice system
        //print(chosenPathNode.pathToTake.name + " was chosen from " + chosenPathNode.name);
       
        //// play sound effect later 
        //if (chosenPathNode.emotionSFX != null)
        //{
        //    sfxPlayer.PlayOneShot(chosenPathNode.emotionSFX);
        //}

        // Called automatically by coroutine if no choice is made?
        if(chosenPathNode == null)
        {
            GameManager.Instance.audioManager.PlayIncorrect();
            character.SetNextStage(Color.red, false);
            foreach(Choice choice in currentChoices)
            {
                if(!choice.isCorrectChoice)
                {
                    dialogueRef.choicesPresent = false;
                    dialogueRef.StartNodeConversation(choice.pathToTake);
                }
            }

            foreach (Transform choice in transform)
            {
                Destroy(choice.gameObject);
            }

            ChoiceOrimage.gameObject.SetActive(false);
            //timerText.text = "";
            StartCoroutine(PlayerAndCharacterReact(false));

            return;
        }

        // Play appropriate Character Reaction
        if(chosenPathNode.isCorrectChoice)
        {
            character.SetNextStage(Color.green, true);
            GameManager.Instance.audioManager.PlayCorrect();
        }
        else
        {
            character.SetNextStage(Color.red, false);
            GameManager.Instance.audioManager.PlayIncorrect();
        }

        if (choiceTimerCoroutine != null)
        {
            StopCoroutine(choiceTimerCoroutine);
            choiceTimerCoroutine = null;
        }

        StartCoroutine(PlayerAndCharacterReact(chosenPathNode.isCorrectChoice));

        //timerText.text = ""; // hide timer when choice is made
        dialogueRef.choicesPresent = false; // gives ability to continue
        //dialogueRef.StopTyping(); // BETTER SOLUTION???
        dialogueRef.StartNodeConversation(chosenPathNode.pathToTake);

        foreach (Transform choice in transform)
        {
            Destroy(choice.gameObject);
        }

        ChoiceOrimage.gameObject.SetActive(false);

    }

    // automatic Fail of the stage
    public void SkipChoice()
    {
        character.SetNextStage(Color.red, false);
    }
}
