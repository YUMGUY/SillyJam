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
    [SerializeField] private float spacing = 100f; // UI spacing (pixels)
    List<RawImage> stages = new List<RawImage>();
    [SerializeField] private int currentStageIndex = 0;
    public enum PersonState {
        Neutral, Hate, Like
    }

    void Start()
    {
        for (int i = 0; i < numStages; i++)
        {
            GameObject stage = Instantiate(talkingStageUI, canvasParent);

            RectTransform rect = stage.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-300f, -spacing * i);

            stages.Add(stage.GetComponent<RawImage>());
        }

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
        if(currentStageIndex == stages.Count - 1)
        {
            print("stage is done");
        }

        currentStageIndex++;

        
    }
}
