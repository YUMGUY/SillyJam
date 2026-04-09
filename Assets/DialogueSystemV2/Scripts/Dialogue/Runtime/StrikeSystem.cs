using UnityEngine;
using UnityEngine.UI;

public class StrikeSystem : MonoBehaviour
{
    [Header("Strike Settings")]
    [SerializeField] private int currentStrikes = 0;
    [SerializeField] private int currentStrikeIndex = 0;

    [Header("UI")]
    [SerializeField] private Image[] strikeBoxes;

    public void RegisterResult(ChoiceResult result)
    {
        if (currentStrikeIndex >= strikeBoxes.Length) return;
        
        if(strikeBoxes.Length == 0)
        {
            Debug.LogWarning("Strike boxes array is empty");
            return;
        }

        Image box = strikeBoxes[currentStrikeIndex];

        switch (result)
        {
            case ChoiceResult.Correct:
                box.color = Color.green;
                currentStrikeIndex++;   // advance - box is filled
                break;

            case ChoiceResult.Incorrect:
                box.color = Color.red;
                currentStrikes++;
                currentStrikeIndex++;   // advance - box is filled
                break;

            case ChoiceResult.Skipped:
                // Do nothing - leave box untouched, move onto next "choice box" stage
                currentStrikeIndex++;
                break;
        }
    }

    public void InitStrikeIndicators()
    {
        // spawn the strikes based on public int # of strikes that is set per scene
    }


    private void UpdateStrikeUI(bool wasCorrect)
    {
        // update your strike indicator visuals here
        // e.g. turn a light red or green
    }

    public void Reset()
    {
        currentStrikes = 0;
        currentStrikeIndex = 0;
        
        foreach (Image box in strikeBoxes)
            box.color = Color.white;
    }
}