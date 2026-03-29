using UnityEngine;

[CreateAssetMenu(fileName = "DialogueBox", menuName = "Scriptable Objects/DialogueBox")]
public class DialogueBox : ScriptableObject
{
    [Header("Conversation Attributes")]
    public Conversation convo;

    [System.Serializable]
    public struct Conversation
    {

        [TextArea(2, 10)]
        public string convoText;

        [Header("-Speaker-")]
        public Speaker character;

        [Header("Character Emotion")]
        public Sprite emote;

        [Header("-Text effects-")]
        public bool autoAdvance;
        public bool ShouldPlayTextSound;
        //public AudioClip textSound;         // This should be combined with "Character"
        public float typingSpeed;


        [Header("-Custom text effects-")]
        public Color textColor_;
        public int inputFontSize;
        public bool textisBold;
        public bool textisWavy;
        public bool textisWiggling;

        [Header("-Sound effect-")]
        public bool ShouldPlaySoundFXInDelay;
        public AudioClip soundToPlay;
        public int SoundAtWhatCharIdx;

        [Header("-Screen shake-")]
        public bool ShouldPlayShake;
        public float intensity_;
        public int ShakeAtWhatCharIdx;
        public float durationShake_;

        [Header("-Screen flash-")]
        public bool ShouldPlayFlash;
        public int FlashAtWhatCharIdx;

        [Header("-Choices-")]
        public bool ShouldLeadToChoice;

        [Header("-List all possible conversation branches from this text-")]
        public Choice[] possibleChoices;



        //[Header("-Character emotions-")]
        //public Sprite[] currentCharacterEmotions;
        //public bool inheritSprite; // node inherits previous sprites
        //[Header("-Enable or Disable Characters / Animation for Characters-")]
        //public Speaker[] activeCharacters; // parallel array with clipsToPlay

        //[Header("-Dialogue Anim-")]
        //public AnimationClip clipToPlay; // for now only one character can "react" at a time for one node

        //[Header("-BGM Effect-")]
        //public bool ShouldUpdateBGM;
        //public int musicAtWhatCharIdx;
        //public AudioClip musicToPlay;

        [Header("Path System Variables")]

        public bool nextNodeInheritCurrent; // inherits name and name color
        public DialogueBox nextNode;
        public bool isEndNode;

    }

    public bool rhythmGameStart;

}
//// Serializable UnityEvent type
//[Serializable]
//public class DialogueEvent : UnityEngine.Events.UnityEvent { }