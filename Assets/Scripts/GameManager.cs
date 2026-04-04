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
            //ResetGameData();
        }
        else
        {
            Instance.ResetGameData();
            Destroy(gameObject);

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
     //   print("Scene count: " + SceneManager.sceneCount.ToString());
    }

    public void ResetGameData()
    {
        // Reset the win/loss criteria
        numPeopleLike = 0;

        // Reset health and state
        currentHealth = maxHealth;
        State = GameState.InProgress; 

        //Debug.Log("GameManager: Data has been reset for a new run.");
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
    }

    public void AddALike()
    {
        print("like aded");
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
        maxNumPeople = 1;
        bool allLike = numPeopleLike >= maxNumPeople;
       // print("numpeoplelike: " + numPeopleLike);
        if (State == GameState.Died)
        {
            State = GameState.BadEnding;
            Debug.Log("Result: Player Died.");
        }
        else if (numPeopleLike >= maxNumPeople) // Using >= is safer than ==
        {
            State = GameState.GoodEnding;
            Debug.Log("Result: Good Ending!");
        }
        else
        {
            State = GameState.BadEnding;
            Debug.Log("Result: Not enough likes.");
        }


        // wait like 3 seconds for now? Do not know agreed way to get to the gameover scene/screen

        // GameOver Scene 
        // always set to the last scene (The Gameover)
        Debug.Log($"LOADING SCENE. Final State: {State}");
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