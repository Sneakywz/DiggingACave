using UnityEngine;

/// <summary>
/// Handles block mining using raycasting.
/// Uses current pickaxe damage from GameManager.
/// </summary>
public class PlayerDig : MonoBehaviour
{
    [SerializeField] private float digRange = 3f;
    
    private Camera cam;
    
    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }
    
    private void Update()
    {
        // Don't allow digging while paused or in shop
        if (GameManager.Instance != null && GameManager.Instance.IsPaused)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, digRange))
            {
                Block block = hit.collider.GetComponent<Block>();
                if (block != null)
                {
                    int damage = GetCurrentDigDamage();
                    block.TakeDamage(damage);
                }
            }
        }
    }
    
    /// <summary>
    /// Get dig damage from current pickaxe.
    /// </summary>
    private int GetCurrentDigDamage()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentPickaxe != null)
        {
            return GameManager.Instance.CurrentPickaxe.DigDamage;
        }
        
        return 1; // Default damage if no pickaxe
    }
}
