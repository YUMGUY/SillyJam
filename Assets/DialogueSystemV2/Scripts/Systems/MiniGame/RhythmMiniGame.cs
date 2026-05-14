using UnityEngine;

public class RhythmMiniGame : MonoBehaviour
{
    [Header("Song Properties")]
    public float timeChange;
    public float BPM;
    // maybe monobehavior
    // put tempo and bpm formula
    // moveo nto next speed at certain time
    [Header("Arriw Properties")]
    public Transform[] spawnTransforms; // parallel array with arrow sprites
    public Sprite[] arrowSprites;
    public float arrowSpeed = 4f;
    public float spawnInterval = 1f; // spawn every second by default
    public GameObject arrow;

}
