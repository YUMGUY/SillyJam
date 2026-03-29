using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    [SerializeField]private SpriteRenderer spriteRenderer;

    public Key keyToPress; // NEW enum (not KeyCode)

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Keyboard.current[keyToPress].wasPressedThisFrame)
        {
            spriteRenderer.color = Color.white;
        }

        if (Keyboard.current[keyToPress].wasReleasedThisFrame)
        {
            spriteRenderer.color = Color.clear;
        }
    }
}