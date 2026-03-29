using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int numPeopleLike = 0;
    private int maxNumPeople = 1;

    public GameState State;

    [Header("Health Settings")]
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float damagePerMiss = 10f;
    public float currentHealth;

    public RhythmGame gameToPlay;
    public AudioManager audioManager;
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
     //   print("Scene count: " + SceneManager.sceneCount.ToString());
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

       // Debug.Log($"Cleaned up {arrows.Length} arrows.");
    }

    public void GameOver()
    {
        switch(State)
        {
            case GameState.Ending:
                Debug.Log("Game is over normally bc stages are completed: " + State.ToString());
                break;
            case GameState.Died:
                Debug.Log("Game is over bc you died.");
                break;
        }

        if(State == GameState.Ending && numPeopleLike == maxNumPeople)
        {
            State = GameState.GoodEnding;
            
        }
        else if((State == GameState.Ending || State == GameState.Died) && numPeopleLike != maxNumPeople)
        {
            State = GameState.BadEnding;
        }

        // wait like 3 seconds for now? Do not know agreed way to get to the gameover scene/screen

        // GameOver Scene 
        // always set to the last scene (The Gameover)
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings -1); 
    }



    public void NoteMissed()
    {
        if(audioManager != null) { audioManager.PlayMiss(); }
        currentHealth -= damagePerMiss;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // Died => Bad Ending
        if (currentHealth <= 0)
        {
            WriteText dialogue = FindAnyObjectByType<WriteText>();
            dialogue.EndConversation();
            StopRhythmGame();
            State = GameState.Died;
            GameOver();
        }
    }

    public void NoteHit()
    {
        if(audioManager != null)
        {
           // audioManager.PlayHit();
        }
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
    Died,
    BadEnding,
    GoodEnding,
    TrueEnding
}