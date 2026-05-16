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
        //bool primaryPressed = Keyboard.current[allowedKeys.primaryKey].wasPressedThisFrame;
        //bool alternatePressed = Keyboard.current[allowedKeys.alternateKey].wasPressedThisFrame;

        //if ((primaryPressed || alternatePressed) && CanBePressed)
        //{
        //    isHit = true;
        //    Destroy(gameObject);
        //}
        if (Keyboard.current[keyToPress].wasPressedThisFrame && CanBePressed)
        {
            isHit = true;
            // GameManager.Instance.NoteHit();
            Destroy(gameObject);
        }

        // Move arrow horizontally to the right
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Activator"))
        {
            CanBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Activator") && isHit == false)
        {
            CanBePressed = false;
        }

        if (collision.CompareTag("Despawner") && isHit == false)
        {
            CanBePressed = false;
            ConversationTimer.Instance.ReduceTime(0.5f);
            Destroy(gameObject);
           // GameManager.Instance.NoteMissed();
        }

    }

}
