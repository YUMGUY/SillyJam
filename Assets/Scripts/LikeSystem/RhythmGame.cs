using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class RhythmGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform[] spawnTransforms; // parallel array with arrow sprites
    public Sprite[] arrowSprites;
    public float arrowSpeed = 4f;
    public float spawnInterval = 1f; // spawn every second by default
    public GameObject arrow;

    private Coroutine spawnRoutine;
    private Key[] keysToPress = { Key.LeftArrow, Key.UpArrow, Key.DownArrow, Key.RightArrow };

    [SerializeField] private float timer = 0f;
    public float timeIntervalToSpeedUp = 12f; // gets overriden by inspector
    void Start()
    {
        if(arrow == null)
        {
            print("Arrow gameobject is null. Please populate.");
        }

        GameManager.Instance.UpdateRhythmGameReference(this);
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeIntervalToSpeedUp)
        {
            spawnInterval = Mathf.Max(0.2f, 0.9f * spawnInterval); // Won't go below 0.2s
            arrowSpeed = Mathf.Min(arrowSpeed + 0.25f, 7f); // Won;t go above 7
            timer = 0f;
        }
    }

    public void SpawnArrow(Vector3 spawnPosition, Key keyToPress, Sprite sprite)
    {
        GameObject spawnedArrow = Instantiate(arrow, spawnPosition, Quaternion.identity);
        ArrowBehavior arrowScript = spawnedArrow.GetComponent<ArrowBehavior>();
        arrowScript.speed = arrowSpeed;
        arrowScript.keyToPress = keyToPress;
        arrowScript.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void StartSpawning()
    {
        spawnRoutine = StartCoroutine(SpawnArrowsRandomly());
    }

    public void StopSpawning()
    {
        if(spawnRoutine != null)
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
            Sprite spriteToUse = arrowSprites[index];
            SpawnArrow(spawnPoint.position, key, spriteToUse);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
