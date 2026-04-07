using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Boundary Box")]
    [SerializeField] private BoxCollider2D boundaryBox;

    [Header("References")]
    [SerializeField] private GameObject playerIcon;
    [SerializeField] private GameObject boundaryVisual; // the box sprite

    private Vector2 _moveInput;
    [SerializeField] private bool _movementEnabled;
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        
    }

    private void Update()
    {
        if (!_movementEnabled)
        {
            _moveInput = Vector2.zero;
            return;
        }

        _moveInput = _moveAction.ReadValue<Vector2>();

        //Vector3 newPos = transform.position + (Vector3)(_moveInput * moveSpeed * Time.deltaTime);

        // Clamp within boundary box
        //if (boundaryBox != null)
        //{
        //    Bounds bounds = boundaryBox.bounds;
        //    newPos.x = Mathf.Clamp(newPos.x, bounds.min.x, bounds.max.x);
        //    newPos.y = Mathf.Clamp(newPos.y, bounds.min.y, bounds.max.y);
        //}

       // transform.position = newPos;
    }

    private void FixedUpdate()
    {
        // 1. Normalize the input to prevent diagonal speed boost
        // .normalized returns a vector with a magnitude of 1
        Vector2 moveDirection = _moveInput.normalized;

        // 2. Apply to linearVelocity
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    public void EnableMovement()
    {
        _movementEnabled = true;
        playerIcon.SetActive(true);
        boundaryVisual.SetActive(true);
    }

    public void DisableMovement()
    {
        _movementEnabled = false;
        playerIcon.SetActive(false);
        boundaryVisual.SetActive(false);
    }
}
