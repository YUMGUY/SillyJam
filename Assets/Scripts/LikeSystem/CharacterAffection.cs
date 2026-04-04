using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAffection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int numStages;
    public PersonState personState = PersonState.Neutral;
    [SerializeField] private GameObject talkingStageUI;
    [SerializeField] private RectTransform canvasParent; // your canvas or panel
    [SerializeField] private float spacing = 120f; // UI spacing (pixels)
    List<TalkingStageInfo> stages = new List<TalkingStageInfo>();
    [SerializeField] private int currentStageIndex = 0;

    public Sprite baseEmote;
    public Sprite badEmote;
    public Sprite goodEmote;

    public RhythmGame rhythmBoard;
    public enum PersonState {
        Neutral, Hate, Like
    }

    void Start()
    {
        for (int i = 0; i < numStages; i++)
        {
            GameObject stage = Instantiate(talkingStageUI, canvasParent);

            RectTransform rect = stage.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-888f, (-spacing * i) + (-206)); // PROBABLY JUST GET A TRASNFORM BUT THIS WILL DO FOR NOW

            stages.Add(stage.GetComponent<TalkingStageInfo>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextStage(Color color, bool isGreen) // Color.Green or Color.Red
    {
        if (currentStageIndex >= stages.Count)
            return;

        stages[currentStageIndex].GetComponent<Image>().color = color;
        stages[currentStageIndex].isGreen = isGreen;
        bool isLastStage = (currentStageIndex == stages.Count - 1);
        currentStageIndex++;
        // Reached final stage. Clean everything up. Will use a ienumerator for changing scenes later
        if (isLastStage)
        {
            GameManager.Instance.StopRhythmGame();

            StartCoroutine(WaitUntilEndDialogueFinishes());
        }
    }

    public IEnumerator WaitUntilEndDialogueFinishes()
    {
        yield return new WaitForSeconds(2f);

        GameManager.Instance.UpdateGameState(GameState.Ending);
        yield return new WaitForEndOfFrame();
        if (DoesCharacterLikePlayer())
        {
            yield return null;
            GameManager.Instance.AddALike();
            yield return null;
        }

        yield return null;
        GameManager.Instance.GameOver();
    }

    public bool DoesCharacterLikePlayer()
    {
        int green = 0;
        int red = 0;
        // Copy the values to a local array first
        bool[] results = new bool[stages.Count];
        for (int i = 0; i < stages.Count; i++)
        {
            results[i] = stages[i].isGreen;
        }

        // Now count the local array
        foreach (bool val in results)
        {
            if (val) green++;
            else red++;
        }
        // print("number of red: " + red);
        return green > red;
    }
}
