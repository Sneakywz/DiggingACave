using UnityEngine;

/// <summary>
/// Controls NPC animations based on movement state.
/// Handles walk, idle, sit, and random emotes.
/// </summary>
[RequireComponent(typeof(Animator))]
public class NPCAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float sitAfterSeconds = 10f; // Time before sitting when idle
    [SerializeField] private float emoteChance = 0.1f; // 10% chance per emote check
    [SerializeField] private float emoteCheckInterval = 5f; // Check for emote every 5 seconds
    
    private Animator animator;
    private float idleTimer = 0f;
    private float emoteTimer = 0f;
    private bool isMoving = false;
    private bool isSitting = false;
    
    // Animation parameter names (ces noms doivent correspondre à ceux dans l'Animator Controller)
    private const string ANIM_WALK = "walk";
    private const string ANIM_IDLE = "idle";
    private const string ANIM_SIT = "sit";
    private const string ANIM_EMOTE_YES = "emote-yes";
    private const string ANIM_EMOTE_NO = "emote-no";
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("NPCAnimationController: No Animator component found!");
            enabled = false;
            return;
        }
        
        // Start with idle animation
        PlayAnimation(ANIM_IDLE);
    }
    
    private void Update()
    {
        if (!isMoving)
        {
            // NPC is idle, increment timer
            idleTimer += Time.deltaTime;
            
            // Check if should sit
            if (!isSitting && idleTimer >= sitAfterSeconds)
            {
                PlayAnimation(ANIM_SIT);
                isSitting = true;
                Debug.Log("NPC sitting down");
            }
            
            // Check for random emotes (but not while sitting)
            if (!isSitting)
            {
                emoteTimer += Time.deltaTime;
                if (emoteTimer >= emoteCheckInterval)
                {
                    TryPlayRandomEmote();
                    emoteTimer = 0f;
                }
            }
        }
    }
    
    /// <summary>
    /// Call this when NPC starts moving.
    /// </summary>
    public void SetMoving(bool moving)
    {
        isMoving = moving;
        
        if (moving)
        {
            // Reset timers
            idleTimer = 0f;
            isSitting = false;
            
            // Play walk animation
            PlayAnimation(ANIM_WALK);
            Debug.Log("NPC walking");
        }
        else
        {
            // NPC stopped moving, play idle
            PlayAnimation(ANIM_IDLE);
            Debug.Log("NPC idle");
        }
    }
    
    /// <summary>
    /// Try to play a random emote animation.
    /// </summary>
    private void TryPlayRandomEmote()
    {
        if (Random.value <= emoteChance)
        {
            // 50/50 chance between yes and no emote
            string emote = Random.value > 0.5f ? ANIM_EMOTE_YES : ANIM_EMOTE_NO;
            PlayAnimation(emote);
            Debug.Log($"NPC playing emote: {emote}");
            
            // Return to idle after emote (you might want to add a delay here)
            Invoke(nameof(ReturnToIdleAfterEmote), 2f);
        }
    }
    
    /// <summary>
    /// Return to idle animation after emote.
    /// </summary>
    private void ReturnToIdleAfterEmote()
    {
        if (!isMoving)
        {
            PlayAnimation(ANIM_IDLE);
        }
    }
    
    /// <summary>
    /// Play an animation by name.
    /// </summary>
    private void PlayAnimation(string animationName)
    {
        if (animator == null) return;
        
        // Try to play the animation
        // Note: This assumes you're using animation triggers or direct play
        // You might need to adjust this based on your Animator Controller setup
        animator.Play(animationName);
    }
    
    /// <summary>
    /// Force play idle animation (called from external scripts).
    /// </summary>
    public void ForceIdle()
    {
        isMoving = false;
        isSitting = false;
        idleTimer = 0f;
        PlayAnimation(ANIM_IDLE);
    }
    
    /// <summary>
    /// Get current moving state.
    /// </summary>
    public bool IsMoving()
    {
        return isMoving;
    }
}