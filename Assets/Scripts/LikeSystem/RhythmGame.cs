using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class RhythmGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform[] spawnTransforms;
    public float arrowSpeed = 5f;
    public float spawnInterval = 1f; // spawn every second by default
    public GameObject arrow;

    private Coroutine spawnRoutine;
    private Key[] keysToPress = { Key.LeftArrow, Key.UpArrow, Key.DownArrow, Key.RightArrow };
    void Start()
    {
        if(arrow == null)
        {
            print("Arrow gameobject is null. Please populate.");
        }

        GameManager.Instance.UpdateRhythmGameReference(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnArrow(Vector3 spawnPosition, Key keyToPress)
    {
        GameObject spawnedArrow = Instantiate(arrow, spawnPosition, Quaternion.identity);
        ArrowBehavior arrowScript = spawnedArrow.GetComponent<ArrowBehavior>();
        arrowScript.speed = arrowSpeed;
        arrowScript.keyToPress = keyToPress;
    }

    public void StartSpawning()
    {
        spawnRoutine = StartCoroutine(SpawnArrowsRandomly());
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnRoutine);
    }

    IEnumerator SpawnArrowsRandomly()
    {
        while (true)
        {
            // Pick a random spawn transform
            int index = Random.Range(0, spawnTransforms.Length);
            Transform spawnPoint = spawnTransforms[index];
            Key key = keysToPress[index];
            SpawnArrow(spawnPoint.position, key);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
