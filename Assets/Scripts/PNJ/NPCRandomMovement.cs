using UnityEngine;

/// <summary>
/// Makes the NPC wander randomly within a defined area.
/// NOW WITH COLLISION DETECTION!
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class NPCRandomMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;
    
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Collision Settings")]
    [SerializeField] private float stuckCheckDistance = 0.5f; // Distance to check if blocked
    [SerializeField] private float stuckTimeout = 3f; // Time before considering stuck
    
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float currentWaitTime = 0f;
    private Rigidbody rb;
    private NPCAnimationController animationController; // NEW: Animation controller reference
    
    // Stuck detection
    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    private void Start()
    {
        // Get or add Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        // Configure Rigidbody for NPC movement
        rb.isKinematic = false; // Allow physics
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Only rotate on Y axis
        rb.mass = 80f; // Human weight
        rb.linearDamping = 5f; // Some friction
        
        // Remember starting position
        startPosition = transform.position;
        lastPosition = transform.position;
        
        // Get animation controller
        animationController = GetComponent<NPCAnimationController>();
        if (animationController == null)
        {
            Debug.LogWarning("NPCRandomMovement: No NPCAnimationController found. Animations will not work.");
        }
        
        // Pick first random destination
        PickNewDestination();
    }

    private void Update()
    {
        if (isWaiting)
        {
            // Wait at current position
            waitTimer += Time.deltaTime;
            
            if (waitTimer >= currentWaitTime)
            {
                // Finished waiting, pick new destination
                isWaiting = false;
                PickNewDestination();
            }
        }
        else
        {
            // Check if stuck
            CheckIfStuck();
            
            // Move towards target
            MoveTowardsTarget();
            
            // Check if reached target
            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance < 0.5f)
            {
                // Reached destination, start waiting
                StartWaiting();
            }
        }
    }

    /// <summary>
    /// Pick a new random destination within the wander radius.
    /// Makes sure the destination is not blocked by a wall.
    /// </summary>
    private void PickNewDestination()
    {
        int attempts = 0;
        bool foundValidDestination = false;
        
        while (!foundValidDestination && attempts < 10)
        {
            // Generate random point within circle
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            Vector3 potentialTarget = startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
            
            // Check if path is blocked by raycasting
            Vector3 direction = (potentialTarget - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, potentialTarget);
            
            // Raycast from current position to target
            if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, distance, LayerMask.GetMask("Default")))
            {
                // Path is clear!
                targetPosition = potentialTarget;
                foundValidDestination = true;
                stuckTimer = 0f; // Reset stuck timer
                
                // Notify animation controller that NPC is moving
                if (animationController != null)
                {
                    animationController.SetMoving(true);
                }
                
                Debug.Log($"NPC picked new destination: {targetPosition}");
            }
            
            attempts++;
        }
        
        // If no valid destination found after 10 attempts, just wait
        if (!foundValidDestination)
        {
            Debug.LogWarning("NPC couldn't find valid destination, waiting instead");
            StartWaiting();
        }
    }

    /// <summary>
    /// Move towards the target position using physics.
    /// </summary>
    private void MoveTowardsTarget()
    {
        // Calculate direction to target (ignore Y axis)
        Vector3 directionToTarget = (targetPosition - transform.position);
        directionToTarget.y = 0;
        
        // Rotate towards target smoothly
        if (directionToTarget.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            // Move using Rigidbody
            Vector3 moveDirection = directionToTarget.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        }
        else
        {
            // Stop moving when close to target
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    /// <summary>
    /// Check if NPC is stuck and pick new destination if needed.
    /// </summary>
    private void CheckIfStuck()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        
        if (distanceMoved < 0.1f)
        {
            // Not moving much, might be stuck
            stuckTimer += Time.deltaTime;
            
            if (stuckTimer >= stuckTimeout)
            {
                Debug.LogWarning("NPC appears to be stuck, picking new destination");
                PickNewDestination();
                stuckTimer = 0f;
            }
        }
        else
        {
            // Moving normally, reset timer
            stuckTimer = 0f;
        }
        
        lastPosition = transform.position;
    }

    /// <summary>
    /// Start waiting at current position.
    /// </summary>
    private void StartWaiting()
    {
        isWaiting = true;
        waitTimer = 0f;
        currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
        
        // Stop moving
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        
        // Notify animation controller that NPC stopped moving
        if (animationController != null)
        {
            animationController.SetMoving(false);
        }
        
        Debug.Log($"NPC waiting for {currentWaitTime:F1} seconds");
    }

    /// <summary>
    /// Draw the wander radius in the editor for visualization.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Vector3 center = Application.isPlaying ? startPosition : transform.position;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, wanderRadius);
        
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPosition, 0.3f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPosition);
        }
    }
}