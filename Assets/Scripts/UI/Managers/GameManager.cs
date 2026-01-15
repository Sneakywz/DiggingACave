using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Main game manager handling resources, pickaxes and game state.
/// Singleton pattern for global access.
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
    [SerializeField] private Vector3 spawnPosition = new Vector3(0, 50, 0);
    
    private List<PickaxeData> ownedPickaxes = new List<PickaxeData>();
    private PickaxeData currentPickaxe;
    private bool isPaused = false;
    
    // Events for UI updates
    public event System.Action<int, int> OnResourcesChanged;
    public event System.Action<PickaxeData> OnPickaxeChanged;
    public event System.Action<bool> OnGamePaused;
    
    public int Coins => coins;
    public int Stones => stones;
    public PickaxeData CurrentPickaxe => currentPickaxe;
    public bool IsPaused => isPaused;
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
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        OnGamePaused?.Invoke(isPaused);
    }
    
    public void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                player.transform.position = spawnPosition;
                controller.enabled = true;
            }
            else
            {
                player.transform.position = spawnPosition;
            }
        }
        
        if (isPaused)
        {
            TogglePause();
        }
    }
    
    public void ResetWorld()
    {
        Debug.Log("Reset World requested");
    }
    
    public void ResetGame()
    {
        coins = 0;
        stones = 0;
        ownedPickaxes.Clear();
        InitializeGame();
        OnResourcesChanged?.Invoke(coins, stones);
        OnPickaxeChanged?.Invoke(currentPickaxe);
        ResetWorld();
    }
    
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuPanel");
    }
}