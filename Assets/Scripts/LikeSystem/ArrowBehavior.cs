using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowBehavior : MonoBehaviour
{
    public float speed = 1.0f;  // Default speed
    public bool CanBePressed;
    public Key keyToPress;
    private bool isHit = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame


    void Update()
    {
        if(Keyboard.current[keyToPress].wasPressedThisFrame && CanBePressed)
        {
            isHit = true;
            GameManager.Instance.NoteHit();
            Destroy(gameObject);       
        }

        // Move arrow horizontally to the right
        transform.position += Vector3.right * speed * Time.deltaTime;
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
            GameManager.Instance.NoteMissed();
        }

    }
}
