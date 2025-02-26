using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 300f;
    public float pushForce = 10f; // Force when running into objects
    public float kickForce = 10f; // Force for the kick action
    public float gravity = -9.81f; // Gravity strength
    public int playerIndex = 0; // 0 for Player 1, 1 for Player 2

    private CharacterController characterController;
    private Vector2 moveInput;
    private PlayerInput playerInput;
    private float verticalVelocity; // Tracks falling speed

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["Move" + playerIndex].performed += OnMove;
        playerInput.actions["Move" + playerIndex].canceled += OnMove;
        playerInput.actions["Kick" + playerIndex].performed += OnKick;
    }

    private void OnDisable()
    {
        playerInput.actions["Move" + playerIndex].performed -= OnMove;
        playerInput.actions["Move" + playerIndex].canceled -= OnMove;
        playerInput.actions["Kick" + playerIndex].performed -= OnKick;
    }

    private void Update()
    {
        MoveCharacter();
        RotateCharacter();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void MoveCharacter()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // Apply gravity inline
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -1f; // Keep grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Fall when not grounded
        }

        move = move * moveSpeed + Vector3.up * verticalVelocity;
        characterController.Move(move * Time.deltaTime);
    }

    private void RotateCharacter()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 forceDirection = hit.moveDirection.normalized;
            float forceMagnitude = moveInput.magnitude * pushForce;
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
    }

    private void OnKick(InputAction.CallbackContext context)
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position + transform.forward, 1f);
        foreach (Collider hit in hitObjects)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * kickForce, ForceMode.Impulse);
            }
        }
    }
feature/powerups

    public void Bounce(float bounceStrength)
    {
        characterController.Move(new Vector3(0, 1 * bounceStrength, 0));
    }
}
=======
}
develop
