using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 300f;
    [SerializeField] private float pushForce = 10f; // Force when running into objects
    [SerializeField] private float kickForce = 10f; // Force for the kick action
    [SerializeField] private int playerIndex = 0; // 0 for Player 1, 1 for Player 2
    [SerializeField] private float _gravityMultiplier = 1f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        RotateCharacter();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        rb.AddForce(Physics.gravity * _gravityMultiplier, ForceMode.Acceleration);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void MoveCharacter()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized * moveSpeed;

        rb.linearVelocity += new Vector3(move.x, 0, move.z);
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
        rb.AddForce(Vector3.up * bounceStrength, ForceMode.Impulse);
    }
}
