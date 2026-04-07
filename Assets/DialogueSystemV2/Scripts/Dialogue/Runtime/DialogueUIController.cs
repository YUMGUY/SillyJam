using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogueUIController : MonoBehaviour, IDialogueUIController
{

    [SerializeField] private GameObject dialogueBox;

    [Header("Choice UI")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject choiceButtonPrefab;

    // future variables for the Choice Timer : the amount of time could be set by the Branch node
    /*
     * HERE
     */

    private bool _choiceMade;
    private IDialogueContext _ctx;
    private List<GameObject> _spawnedButtons = new List<GameObject>();

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
        choicePanel.SetActive(true);

        // Clear any old buttons
        foreach (var btn in _spawnedButtons)
            Destroy(btn);
        _spawnedButtons.Clear();

        // Spawn a button per choice
        for (int i = 0; i < choices.Length; i++)
        {
            int capturedIndex = i; // capture for lambda
            GameObject btnObj = Instantiate(choiceButtonPrefab, choicePanel.transform);

            // Set label text
            TMP_Text label = btnObj.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = choices[capturedIndex].label;

            // Wire up click
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => OnChoiceSelected(choices[capturedIndex], capturedIndex));

            _spawnedButtons.Add(btnObj);
        }

        // Wait until player picks
        yield return new WaitUntil(() => _choiceMade);

        // Clean up
        choicePanel.SetActive(false);
        foreach (var btn in _spawnedButtons)
            Destroy(btn);
        _spawnedButtons.Clear();
    }

    private void OnChoiceSelected(DialogueChoice choice,int index)
    {
        if(_ctx == null) { Debug.Log("Missing dialogue Context"); }

        if(choice.isCorrectChoice)
        {
            Debug.Log("You made the correct choice.");
        }

        _ctx.LastChosenIndex = index;
        _choiceMade = true;
    }
}