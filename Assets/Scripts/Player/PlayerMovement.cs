using UnityEngine;

/// <summary>
/// FPS movement using CharacterController.
/// Supports AZERTY (ZQSD) and QWERTY (WASD).
/// Jump works even when standing still.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        velocity = Vector3.zero;
    }

    private void Update()
    {
        HandleMovementAndJump();
    }

    /// <summary>
    /// Handles movement, gravity and jump in a single pass.
    /// </summary>
    private void HandleMovementAndJump()
    {
        float forward = 0f;
        float right = 0f;

        // Forward / backward (AZERTY + QWERTY)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
            forward += 1f;
        if (Input.GetKey(KeyCode.S))
            forward -= 1f;

        // Strafe left / right
        if (Input.GetKey(KeyCode.D))
            right += 1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
            right -= 1f;

        Vector3 move =
            transform.forward * forward +
            transform.right * right;

        // Ground check
        if (controller.isGrounded)
        {
            if (velocity.y < 0f)
                velocity.y = -2f;

            // Jump works even if move == Vector3.zero
            if (Input.GetKeyDown(KeyCode.Space))
                velocity.y = jumpForce;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Final movement vector (horizontal + vertical)
        Vector3 finalMove =
            move.normalized * moveSpeed +
            Vector3.up * velocity.y;

        controller.Move(finalMove * Time.deltaTime);
    }
}
