using System.Collections;
using System.Collections.Generic;
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
    public LaneKeys test;

    [Header("Song Properties")]
    public float timeChange;
    public float BPM;
    public float beatRate = 1f;
    public bool hasTempoChanged;
    public bool isRunning;

    [Header("Arrow Properties")]
    public GameObject arrow;
    public Transform[] spawnTransforms; // parallel array with arrow sprites
    public Sprite[] arrowSprites;
    public float arrowSpeed = 4f;
    public float spawnInterval = 1f; // spawn every second by default

    [Header("VFX")]
    public ParticleSystem[] hitParticles;

    [SerializeField] private float timer = 0f;
    private Coroutine spawnRoutine;
    private Key[] keysToPress = { Key.A, Key.S, Key.W, Key.D };

    private LaneKeys[] lanes = new LaneKeys[]
        {
            new LaneKeys { primaryKey = Key.A, alternateKey = Key.LeftArrow},  
            new LaneKeys { primaryKey = Key.S, alternateKey = Key.DownArrow },  
            new LaneKeys { primaryKey = Key.W, alternateKey = Key.UpArrow },    
            new LaneKeys { primaryKey = Key.D, alternateKey = Key.RightArrow }
        };
    
    public static RhythmMiniGame Instance { get; private set; }

    // New: Dictionary to track active, pressable arrows by key
    private Dictionary<Key, List<ArrowBehavior>> activeArrows = new Dictionary<Key, List<ArrowBehavior>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Initialize the dictionary for all possible keys
        foreach (Key key in keysToPress)
        {
            activeArrows[key] = new List<ArrowBehavior>();
        }
    }

    void Update()
    {
        if(isRunning)
            timer += Time.deltaTime;

        if (timer >= timeChange && !hasTempoChanged)
        {
            hasTempoChanged = true;
            arrowSpeed *= 1.25f;
        }

        // Centralized input handling: Check all tracked keys
        foreach (var entry in activeArrows)
        {
            Key key = entry.Key;
            
            // If the key was pressed, trigger the hit logic on the oldest active arrow
            if (Keyboard.current[key].wasPressedThisFrame && entry.Value.Count > 0)
            {
                ArrowBehavior hitArrow = entry.Value[0];
                hitArrow.HandleHit();
            }
        }
    }

    // Registration methods called by ArrowBehavior
    public void RegisterArrow(Key key, ArrowBehavior arrow)
    {
        if (activeArrows.ContainsKey(key))
            activeArrows[key].Add(arrow);
    }

    public void UnregisterArrow(Key key, ArrowBehavior arrow)
    {
        if (activeArrows.ContainsKey(key))
            activeArrows[key].Remove(arrow);
    }

    public void PlayHitEffect(Key key)
    {
        // Find the lane index for the key
        int index = -1;
        for (int i = 0; i < keysToPress.Length; i++)
        {
            if (keysToPress[i] == key)
            {
                index = i;
                break;
            }
        }
        // If the index is valid and the particle system is assigned, play it
        if (index >= 0 && index < hitParticles.Length && hitParticles[index] != null)
        {
            hitParticles[index].Play();
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