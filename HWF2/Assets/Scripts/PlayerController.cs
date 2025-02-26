using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 300f;
    public float kickForce = 10f;
    public int playerIndex = 0; // 0 for Player 1, 1 for Player 2

    private CharacterController characterController;
    private Vector2 moveInput;
    private PlayerInput playerInput;

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
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.Move(move * moveSpeed * Time.deltaTime);
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

    public void Bounce(float bounceStrength)
    {
        characterController.Move(new Vector3(0, 1 * bounceStrength, 0));
    }
}
