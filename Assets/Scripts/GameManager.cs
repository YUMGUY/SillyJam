using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int numPeopleLike = 0;
    private int maxNumPeople = 3;

    public GameState State;

    [Header("Health Settings")]
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float damagePerMiss = 10f;
    public float currentHealth;

    public RhythmGame gameToPlay;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
    }

    public void AddALike()
    {
        ++numPeopleLike;
    }

    public void StartRhythmGame()
    {
        if (gameToPlay != null)
            gameToPlay.StartSpawning();
        
    }

    public void StopRhythmGame()
    {
        if (gameToPlay != null)
        {
            gameToPlay.StopSpawning();
        }

        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");

        foreach (GameObject arrow in arrows)
        {
            Destroy(arrow);
        }

        Debug.Log($"Cleaned up {arrows.Length} arrows.");
    }

    public void GameOver()
    {
        // open the game over scene if reached end?
        Debug.Log("Game is over.");
    }

    public void NoteMissed()
    {
        Debug.Log("Missed note");
        currentHealth -= damagePerMiss;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // Bad Ending? or go straight to judging
        if (currentHealth <= 0)
        {
            WriteText dialogue = FindAnyObjectByType<WriteText>();
            dialogue.EndConversation();
            StopRhythmGame();
            State = GameState.Ending;
            GameOver();
        }
    }

    public void NoteHit()
    {
        //
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }
    public void UpdateUIReference(Slider newSlider)
    {
        healthSlider = newSlider;
        currentHealth = maxHealth;
        UpdateHealthUI(); // Refresh the bar immediately so it's not empty/full
    }

    public void UpdateRhythmGameReference(RhythmGame game)
    {
        gameToPlay = game;
    }
}

public enum GameState
{
    InProgress,
    Ending,
    BadEnding,
    GoodEnding,
    TrueEnding
}