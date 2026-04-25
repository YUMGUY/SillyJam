using UnityEngine;
using UnityEngine.UI;

public class StrikeSystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image[] strikeBoxes;


    [Header("Strike Counts")]
    [SerializeField] private int _goodIndex = 0;
    [SerializeField] private int _badIndex = 0;

    [Header("Good Strike UI")]
    [SerializeField] private Image[] goodBoxes;
    [SerializeField] private Sprite goodDefault;
    [SerializeField] private Sprite goodActive;
  


    [Header("Bad Strike UI")]
    [SerializeField] private Image[] badBoxes;
    [SerializeField] private Sprite badDefault;
    [SerializeField] private Sprite badActive;

    private DialogueRunner dialogueRunner;

    private void Start()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
    }

    // at the end, could do by majority rule if time runs out
    public void RegisterResult(ChoiceResult result)
    {
        switch (result)
        {
            case ChoiceResult.Correct:
                if (_goodIndex < goodBoxes.Length)
                {
                    goodBoxes[_goodIndex].sprite = goodActive;
                    _goodIndex++;
                    if (_goodIndex == goodBoxes.Length)
                    {
                        dialogueRunner.ForceEndDialogue(); // start ending sequence
                    }
                }
                break;

            case ChoiceResult.Incorrect:
                if (_badIndex < badBoxes.Length)
                {
                    badBoxes[_badIndex].sprite = badActive;
                    _badIndex++;
                    if (_badIndex == badBoxes.Length)
                    {
                        dialogueRunner.ForceEndDialogue();
                    }

                }
                break;

            case ChoiceResult.Skipped:
                // no box filled, no index advanced
                break;
        }
    }

    public void InitStrikeIndicators()
    {
        // spawn the strikes based on public int # of strikes that is set per scene
    }


    public void Reset()
    {
        _goodIndex = 0;
        _badIndex = 0;

        foreach (Image box in goodBoxes)
            box.sprite = goodDefault;

        foreach (Image box in badBoxes)
            box.sprite = badDefault;
    }

    private DialogueEndResult EvaluateEnding()
    {
        bool allGood = _goodIndex >= goodBoxes.Length;
        bool allBad = _badIndex >= badBoxes.Length;

        DialogueEndResult result;

        if (allGood)
            result = DialogueEndResult.Good;
        else if (allBad)
            result = DialogueEndResult.Bad;
        else
            result = DialogueEndResult.Mediocre;

        Debug.Log("Dialogue result: " + result);
        return result;
    }
}

public enum DialogueEndResult
{
    Good,
    Bad,
    Mediocre
}