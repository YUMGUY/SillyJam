using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class RhythmMiniGame : MonoBehaviour
{
    // TODO use this later
    [System.Serializable]
    public struct LaneKeys
    {
        public Key primaryKey;   // e.g., Key.A
        public Key alternateKey; // e.g., Key.LeftArrow
    }

    [Header("Song Properties")]
    public float timeChange;
    public float BPM;
    public float beatRate = 1f;
    public bool hasTempoChanged;
    public bool isRunning;

    // put tempo and bpm formula
    // moveo nto next speed at certain time
    [Header("Arrow Properties")]
    public GameObject arrow;
    public Transform[] spawnTransforms; // parallel array with arrow sprites
    public Sprite[] arrowSprites;
    public float arrowSpeed = 4f;
    public float spawnInterval = 1f; // spawn every second by default


    private Coroutine spawnRoutine;
    private Key[] keysToPress = { Key.A, Key.S, Key.W, Key.D };

    private LaneKeys[] lanes = new LaneKeys[]
        {
            new LaneKeys { primaryKey = Key.A, alternateKey = Key.LeftArrow},  
            new LaneKeys { primaryKey = Key.S, alternateKey = Key.DownArrow },  
            new LaneKeys { primaryKey = Key.W, alternateKey = Key.UpArrow },    
            new LaneKeys { primaryKey = Key.D, alternateKey = Key.RightArrow }
        };
    [SerializeField] private float timer = 0f;

    void Update()
    {
        if(isRunning)
            timer += Time.deltaTime;

        if (timer >= timeChange && !hasTempoChanged)
        {
            hasTempoChanged = true;
            arrowSpeed *= 1.25f;
        }
    }


    public void SpawnArrow(Vector3 spawnPosition, Key key, Sprite sprite)
    {
        GameObject spawnedArrow = Instantiate(arrow, spawnPosition, Quaternion.identity);
        ArrowBehavior arrowScript = spawnedArrow.GetComponent<ArrowBehavior>();
        arrowScript.speed = arrowSpeed;
        arrowScript.keyToPress = key;
        arrowScript.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void StartSpawning()
    {
        isRunning = true;
        spawnRoutine = StartCoroutine(SpawnArrowsRandomly());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            isRunning = false;
            StopCoroutine(spawnRoutine);
        }
    }

    IEnumerator SpawnArrowsRandomly()
    {
        float spawnInterval_ = (60f / BPM) / beatRate;

        while (true)
        {
            
            // Pick a random spawn transform
            int index = Random.Range(0, spawnTransforms.Length);
            Transform spawnPoint = spawnTransforms[index];
            Key key = keysToPress[index];
            LaneKeys laneKeyData = lanes[index];
            Sprite spriteToUse = arrowSprites[index];
            SpawnArrow(spawnPoint.position, key, spriteToUse);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}