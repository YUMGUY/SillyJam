using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneLoader : MonoBehaviour
{
    public int sceneIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(sceneIndex == 0)
        {
            Debug.Log("Scene loader does not have a scene index");
        }
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}
