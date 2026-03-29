using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject goodEnding;
    public GameObject badEnding;
    void Start()
    {
        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.State == GameState.GoodEnding)
            {
                goodEnding.SetActive(true);
            }
            else { badEnding.SetActive(true); }
            print("You got an ending. If this message doesn't show up. Something went wrong with the GameManager.");
        }
    }


}
