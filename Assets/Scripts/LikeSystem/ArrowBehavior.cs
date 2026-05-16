using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowBehavior : MonoBehaviour
{
    public float speed = 1.0f;  // Default speed
    public bool CanBePressed;
    public Key keyToPress;
    private bool isHit = false;
    public RhythmMiniGame.LaneKeys allowedKeys;

    void Start()
    {
        
    }

    void Update()
    {
        // Move arrow down
        transform.position += speed * Time.deltaTime * Vector3.down;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Activator"))
        {
            CanBePressed = true;
            // Tell the manager: "I'm ready to be hit!"
            RhythmMiniGame.Instance.RegisterArrow(keyToPress, this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Activator") && isHit == false)
        {
            CanBePressed = false;
            // Tell the manager: "I'm leaving the zone unhit"
            RhythmMiniGame.Instance.UnregisterArrow(keyToPress, this);
        }

        if (collision.CompareTag("Despawner") && isHit == false)
        {
            CanBePressed = false;
            ConversationTimer.Instance.ReduceTime(0.5f);
            if (AudioHub.Instance != null)
                AudioHub.Instance.PlayBeatMissed();
            Destroy(gameObject);
        }
    }

    // Manager calls this when input is detected for this arrow
    public void HandleHit()
    {
        if (isHit) return;

        isHit = true;
        
        // Ensure we unregister before destroying
        RhythmMiniGame.Instance.UnregisterArrow(keyToPress, this);
        // Trigger the particle system in the manager based on the lane key
        RhythmMiniGame.Instance.PlayHitEffect(keyToPress);

        Destroy(gameObject);
    }
}
