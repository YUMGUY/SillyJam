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
    [SerializeField] private float choiceTimeLimit = 5f;

    // show a timer here
    void Start()
    {
        // Make sure debugging text doesnt show
        timerText.text = "";
    }

    private void Update()
    {
    }

    IEnumerator ChoiceTimer()
    {
        int timeRemaining = Mathf.CeilToInt(choiceTimeLimit);
        timerText.text = timeRemaining.ToString();
       
        while (timeRemaining > 0)
        {
            timerText.text = timeRemaining.ToString();

            yield return new WaitForSeconds(1f);

            timeRemaining--;
        }

        timerText.text = ""; // clear or show "0" if you want

        ChoosePath(null);
    }

    public void DisplayChoices(int numChoices, Choice[] choices)
    {
        currentChoices = choices; // copy
        float startY = -150f;
        for (int i = 0; i < choices.Length; ++i)
        {
            int dindex = i;
            Button buttonMade = Instantiate(choiceButtonPrefab, transform);
            buttonMade.onClick.AddListener(delegate { ChoosePath(choices[dindex]); });
            // position buttons
            buttonMade.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, startY);
            startY += 150;
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
       
        //// change emotion
        //if (chosenPathNode.changeEmotionValue == true)
        //{
        //    BattleManager.instance.ChangeEmotion(chosenPathNode.emotionValue, chosenPathNode.emotion);
        //}
        //// play sound effect later 
        //if (chosenPathNode.emotionSFX != null)
        //{
        //    sfxPlayer.PlayOneShot(chosenPathNode.emotionSFX);
        //}

        // Called automatically by coroutine if no choice is made?
        if(chosenPathNode == null)
        {
            character.SetNextStage(Color.red);
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

            return;
        }

        if(chosenPathNode.isCorrectChoice)
        {
            character.SetNextStage(Color.green);
        }
        else
        {
            character.SetNextStage(Color.red);
        }

        if (choiceTimerCoroutine != null)
        {
            StopCoroutine(choiceTimerCoroutine);
            choiceTimerCoroutine = null;
        }

        timerText.text = ""; // hide timer when choice is made
        dialogueRef.choicesPresent = false; // gives ability to continue
        //dialogueRef.StopTyping(); // BETTER SOLUTION???
        dialogueRef.StartNodeConversation(chosenPathNode.pathToTake);


        foreach (Transform choice in transform)
        {
            Destroy(choice.gameObject);
        }
    }

    // atuomatic Fail of the stage
    public void SkipChoice()
    {
       // character.numStages++;
        character.SetNextStage(Color.red);
    }

}
