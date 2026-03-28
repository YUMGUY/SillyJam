using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class WriteText : MonoBehaviour
{



    [Header("Nodal Dialogue UI")]
    public GameObject mainPanel;
    public TextMeshProUGUI textPanel;
    public TextMeshProUGUI namePanel;
    public DialogueBox currentNode;
    public DialogueBox startNode;

    public float currentTypingSpeed;
    public float timeUntilNextDialogue;
    [SerializeField] private float defaultTypingSpeed;
    [Header("Choice System Factor")]
    public bool choicesPresent;
    public ChoiceSystem choiceSysRef;

    private Coroutine NodeTypingCoroutine = null;
    private State state = State.TALKING;

    private enum State
    {
        TALKING, COMPLETED, AWAITING_REPLY, FINISHED_CONVERSATION
    }
    
    // This gives us control on when to start the conversation for now
    private void OnEnable()
    {
        print("Enabled"); // this works
        // start conversation
        state = State.TALKING;
        StartNodeConversation(startNode);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.COMPLETED) // if the line is completed
        {

            if (choicesPresent == true)
            {
                print("Cannot continue when choices are present");
                return;
            }

            // Takes care of end node. Starts the typing again
            GoNextNode();
        }
        //else // in the midst of typing
        //{
        //    // StopAllCoroutines(); // rather than stop all coroutines that could potentially happen here
        //    //if (ifPlayingIntroAnim == true)
        //    //{
        //    //    print("Safety measure against completing text while animating");
        //    //    return;
        //    //}

        //    //textPanel.text = currentNode.convo.convoText;

        //    // this code will need to be fixed
        //    if (choicesPresent == true)
        //    {
        //       // choiceSysRef.DisplayChoices(currentNode.convo.possibleChoices.Length, currentNode.convo.possibleChoices);
        //    }
        //    state = State.COMPLETED;
        //   // state = State.AWAITING_REPLY;
        //}

    }


    public void StartNodeConversation(DialogueBox node)
    {
        // provides choices a way to start a new path
        currentNode = node;
        state = State.TALKING;
        NodeTypingCoroutine = StartCoroutine(TypeNodeText(node.convo.convoText));
    }

    private IEnumerator TypeNodeText(string text)
    {
        // Reset State of TextBox
        textPanel.text = "";
        state = State.TALKING;

        // Choice System 
        choicesPresent = currentNode.convo.ShouldLeadToChoice;

        int charIndex = 0;
        // name panel, emotions, text, typing speed
        CheckNode();

        //if (currentNode.convo.animationDelay == true)
        //{
        //    // start animation coroutine that would clear ui 
        //    yield return StartCoroutine(PlaySpecialAnimation(currentNode.convo.clipToPlay));
        //}
        while (state != State.COMPLETED)
        {
            // add audio clip of text here
            //PlaySoundFX(charIndex);

            // play animation for character
            //try
            //{
            //    if (currentNode.convo.animationDelay == false && currentNode.convo.clipToPlay != null && currentNode.convo.AnimationAtWhatCharIdx == charIndex)
            //    {
            //        Animator charToAnimate = characterControllerRef.FindCharacterAnimator(currentNode.convo.character);
            //        charToAnimate.Play(currentNode.convo.clipToPlay.name, 0, 0);
            //    }
            //}
            //catch
            //{
            //    print("Something went wrong with the animation clip");
            //}

            textPanel.text += text[charIndex];
            yield return new WaitForSeconds(currentTypingSpeed);
            if (++charIndex == text.Length)
            {
                yield return new WaitForSeconds(timeUntilNextDialogue);
                state = State.COMPLETED;
            }
        }

        // Depends on choices present set before the typing starts
        if (choicesPresent == true)
        {
            //print("show choice");
            choiceSysRef.DisplayChoices(currentNode.convo.possibleChoices.Length, currentNode.convo.possibleChoices);
            state = State.AWAITING_REPLY;
        }

        yield return null;
    }

    public void GoNextNode() // controls the value of convoIndex, which is the marker for which speaker is speaking
    {
        if (currentNode.convo.nextNode != null)
        {
            textPanel.text = string.Empty;
            currentNode = currentNode.convo.nextNode;
            StartNodeConversation(currentNode);
            // NodeTypingCoroutine = StartCoroutine(TypeNodeText(currentNode.convo.convoText)); // ASSIGN COROUTINE *** acts as safety measure rather than stopping all coroutines
        }
        // ERROR CATCHING
        else if (currentNode.convo.nextNode == null && currentNode.convo.isEndNode != true)
        {
            print("ERROR: CURRENT NODE " + currentNode.name + " does not have a nextNode");
        }
        // End of Dialogue
        else
        {
            print("end of dialogue for now");
            state = State.FINISHED_CONVERSATION;
            // create function to invoke stored coroutines, switch system, etc
        }
    }

    public void CheckNode()
    {
        // check typing speed
        try
        {
            if (currentNode.convo.typingSpeed <= 0.0f)
            {
                currentTypingSpeed = defaultTypingSpeed;
            }
            else
            {
                currentTypingSpeed = currentNode.convo.typingSpeed;
            }
        }
        catch { print("Typing Speed is defaulted"); }
        // check Character Name & Character Name Color & textColor
        try
        {
            namePanel.text = currentNode.convo.character.speakerName;
            namePanel.color = currentNode.convo.character.nameColor;
            if (currentNode.convo.textColor_.a <= 0)
            {
                textPanel.color = Color.white;
            }
            else
            {
                textPanel.color = currentNode.convo.textColor_;
            }
        }
        catch
        {
            print("Error at: " + currentNode.name + " , has missing speaker");
            namePanel.text = "???";
            namePanel.color = Color.white;
        }
        //// check Character Sprite
        //try
        //{
        //    if (currentNode.convo.inheritSprite == false || currentNode.convo.currentCharacterEmotions.Length != 0)
        //    {
        //        characterControllerRef.ShowCharacterEmotions(currentNode.convo.character.speakerName, currentNode.convo
        //       .currentCharacterEmotions[0]);
        //    }
        //    else { } // do nothing to change sprites
        //}
        //catch
        //{
        //    print("ERROR: Character Sprite Array for " + currentNode.name + " is empty");
        //}

    }



}
