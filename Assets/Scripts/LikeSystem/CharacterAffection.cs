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
    List<Image> stages = new List<Image>();
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

            stages.Add(stage.GetComponent<Image>());
        }

        //GameManager.Instance.StartRhythmGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextStage(Color color) // Color.Green or Color.Red
    {
        if (currentStageIndex >= stages.Count)
            return;

        stages[currentStageIndex].color = color;

        // Reached final stage. Clean everything up. Will use a ienumerator for changing scenes later
        if(currentStageIndex == stages.Count - 1)
        {
            GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");

            foreach (GameObject arrow in arrows)
            {
                Destroy(arrow);
            }
            StartCoroutine(WaitUntilEndDialogueFinishes());
            //GameManager.Instance.StopRhythmGame();
            //GameManager.Instance.UpdateGameState(GameState.Ending);
            //if(DoesCharacterLikePlayer())
            //{
            //    GameManager.Instance.AddALike();
            //}
            //GameManager.Instance.GameOver();
        }

        currentStageIndex++;
    }

    public IEnumerator WaitUntilEndDialogueFinishes()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.StopRhythmGame();
        GameManager.Instance.UpdateGameState(GameState.Ending);
        if (DoesCharacterLikePlayer())
        {
            GameManager.Instance.AddALike();
        }

        GameManager.Instance.GameOver();
    }

    public bool DoesCharacterLikePlayer()
    {
        int green = 0;
        int red = 0;
        foreach (Image stage in stages)
        {
            if(stage.color == Color.green)
            {
                ++green;
            }
            else { ++red; }
        }


        return green > red;
    }
}
