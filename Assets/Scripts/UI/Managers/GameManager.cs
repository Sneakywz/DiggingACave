using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Main game manager handling resources, pickaxes and game state.
/// Singleton pattern for global access.
/// NOW WITH WORKING RESPAWN, RESET WORLD AND RESET GAME!
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Resources")]
    [SerializeField] private int coins = 0;
    [SerializeField] private int stones = 0;
    
    [Header("Pickaxes")]
    [SerializeField] private List<PickaxeData> availablePickaxes = new List<PickaxeData>();
    
    [Header("Respawn")]
    [SerializeField] private Vector3 spawnPosition = new Vector3(-1.05f, -1f, -25.41f);
    
    private List<PickaxeData> ownedPickaxes = new List<PickaxeData>();
    private PickaxeData currentPickaxe;
    private bool isPaused = false;
    private bool isShopOpen = false;
    
    // Events for UI updates
    public event System.Action<int, int> OnResourcesChanged;
    public event System.Action<PickaxeData> OnPickaxeChanged;
    public event System.Action<bool> OnGamePaused;
    public event System.Action<bool> OnShopToggled;
    
    public int Coins => coins;
    public int Stones => stones;
    public PickaxeData CurrentPickaxe => currentPickaxe;
    public bool IsPaused => isPaused;
    public bool IsShopOpen => isShopOpen;
    public List<PickaxeData> AvailablePickaxes => availablePickaxes;
    public List<PickaxeData> OwnedPickaxes => ownedPickaxes;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeGame();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    private void InitializeGame()
    {
        if (availablePickaxes.Count > 0)
        {
            currentPickaxe = availablePickaxes[0];
            ownedPickaxes.Add(currentPickaxe);
        }
    }
    
    public void AddCoins(int amount)
    {
        coins += amount;
        OnResourcesChanged?.Invoke(coins, stones);
    }
    
    public void AddStones(int amount)
    {
        stones += amount;
        OnResourcesChanged?.Invoke(coins, stones);
    }
    
    public bool PurchasePickaxe(PickaxeData pickaxe)
    {
        if (ownedPickaxes.Contains(pickaxe))
            return false;
        
        if (coins >= pickaxe.CoinPrice && stones >= pickaxe.StonePrice)
        {
            coins -= pickaxe.CoinPrice;
            stones -= pickaxe.StonePrice;
            ownedPickaxes.Add(pickaxe);
            OnResourcesChanged?.Invoke(coins, stones);
            return true;
        }
        
        return false;
    }
    
    public void EquipPickaxe(PickaxeData pickaxe)
    {
        if (ownedPickaxes.Contains(pickaxe))
        {
            currentPickaxe = pickaxe;
            OnPickaxeChanged?.Invoke(currentPickaxe);
        }
    }
    
    public bool OwnsPickaxe(PickaxeData pickaxe)
    {
        return ownedPickaxes.Contains(pickaxe);
    }
    
    public void TogglePause()
    {
        // Don't allow pausing while shop is open
        if (isShopOpen)
            return;
            
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        OnGamePaused?.Invoke(isPaused);
    }
    
    public void SetShopOpen(bool open)
    {
        isShopOpen = open;
        OnShopToggled?.Invoke(isShopOpen);
        Debug.Log($"GameManager: Shop {(open ? "opened" : "closed")}");
    }
    
    /// <summary>
    /// FIXED: Respawn player at spawn position with better error handling
    /// </summary>
    public void RespawnPlayer()
    {
        Debug.Log("=== RESPAWN REQUESTED ===");
        
        // Try to find player by tag first
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        // If not found by tag, try to find by name
        if (player == null)
        {
            Debug.LogWarning("Player not found by tag 'Player', searching by name...");
            player = GameObject.Find("Player");
        }
        
        // If still not found, try to find PlayerMovement component
        if (player == null)
        {
            Debug.LogWarning("Player not found by name, searching for PlayerMovement...");
            PlayerMovement movement = FindObjectOfType<PlayerMovement>();
            if (movement != null)
            {
                player = movement.gameObject;
            }
        }
        
        if (player != null)
        {
            Debug.Log($"Player found: {player.name}");
            
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Disable controller to teleport
                controller.enabled = false;
                player.transform.position = spawnPosition;
                controller.enabled = true;
                Debug.Log($"Player respawned at {spawnPosition}");
            }
            else
            {
                player.transform.position = spawnPosition;
                Debug.Log($"Player respawned at {spawnPosition} (no CharacterController)");
            }
        }
        else
        {
            Debug.LogError("❌ PLAYER NOT FOUND! Make sure your player GameObject has:");
            Debug.LogError("  1. Tag 'Player' OR");
            Debug.LogError("  2. Name 'Player' OR");
            Debug.LogError("  3. PlayerMovement component");
        }
        
        // Unpause if paused
        if (isPaused)
        {
            TogglePause();
        }
    }
    
    /// <summary>
    /// FIXED: Reset world by regenerating all blocks
    /// </summary>
    public void ResetWorld()
    {
        Debug.Log("=== RESET WORLD REQUESTED ===");
        
        WorldGenerator worldGen = FindObjectOfType<WorldGenerator>();
        
        if (worldGen != null)
        {
            worldGen.RegenerateWorld();
            Debug.Log("✅ World regenerated successfully!");
        }
        else
        {
            Debug.LogError("❌ WorldGenerator not found in scene!");
            Debug.LogError("Make sure you have a GameObject with the WorldGenerator script.");
        }
    }
    
    /// <summary>
    /// FIXED: Reset game completely (resources + world)
    /// </summary>
    public void ResetGame()
    {
        Debug.Log("=== RESET GAME REQUESTED ===");
        
        // Reset resources
        coins = 0;
        stones = 0;
        
        // Reset pickaxes
        ownedPickaxes.Clear();
        InitializeGame();
        
        // Notify UI
        OnResourcesChanged?.Invoke(coins, stones);
        OnPickaxeChanged?.Invoke(currentPickaxe);
        
        // Reset world
        ResetWorld();
        
        // Respawn player
        RespawnPlayer();
        
        Debug.Log("✅ Game reset complete!");
    }
    
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuPanel");
    }
}