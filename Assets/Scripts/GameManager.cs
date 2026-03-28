using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int numPeopleLike = 0;
    private int maxNumPeople = 3;

    public GameState State;
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

    public void GameOver()
    {
        // open the game over scene if reached end?
    }

}

public enum GameState
{
    InProgress,
    BadEnding,
    GoodEnding,
    TrueEnding
}