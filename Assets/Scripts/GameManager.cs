using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

}

public enum GameState
{
    InProgress,
    BadEnding,
    GoodEnding,
    TrueEnding
}