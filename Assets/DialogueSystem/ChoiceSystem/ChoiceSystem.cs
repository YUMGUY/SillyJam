using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button choiceButtonPrefab;
    [SerializeField] private WriteText dialogueRef;
    [SerializeField] private CharacterAffection character;
    [SerializeField] private DialogueBox noChoiceMadePath;
    void Start()
    {
        // start the timer (5 seconds)
    }

    private void Update()
    {
        // when timer reaches <= 0
        // start the failed to choose node conversation
    }

    public void DisplayChoices(int numChoices, Choice[] choices)
    {
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

        if(chosenPathNode.isCorrectChoice)
        {
            character.numStages++;
            character.SetNextStage(Color.green);
        }
        else
        {
            character.numStages++;
            character.SetNextStage(Color.red);
        }

        dialogueRef.choicesPresent = false; // gives ability to continue
        //dialogueRef.StopTyping(); // BETTER SOLUTION???
        dialogueRef.StartNodeConversation(chosenPathNode.pathToTake);


        foreach (Transform choice in transform)
        {
            Destroy(choice.gameObject);
        }
    }

}
