using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Handles NPC shop UI for purchasing and equipping pickaxes.
/// Allows buying new pickaxes and re-equipping owned ones for free.
/// NOW NOTIFIES GAMEMANAGER WHEN OPEN/CLOSED!
/// </summary>
public class ShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform pickaxeListContainer;
    [SerializeField] private GameObject pickaxeItemPrefab;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button sellStonesButton;
    
    [Header("Current Pickaxe Display")]
    [SerializeField] private TextMeshProUGUI currentPickaxeText;
    [SerializeField] private Image currentPickaxeIcon;
    
    private List<PickaxeItemUI> pickaxeItems = new List<PickaxeItemUI>();
    private bool isShopOpen = false;
    
    private void Start()
    {
        Debug.Log("=== SHOPUI START ===");
        Debug.Log($"ShopPanel: {(shopPanel != null ? "OK" : "NULL")}");
        Debug.Log($"GameManager: {(GameManager.Instance != null ? "OK" : "NULL")}");
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseShop);
        }
        
        if (sellStonesButton != null)
        {
            sellStonesButton.onClick.AddListener(SellAllStones);
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPickaxeChanged += UpdateCurrentPickaxe;
            GameManager.Instance.OnResourcesChanged += OnResourcesChanged;
            Debug.Log($"Available Pickaxes: {GameManager.Instance.AvailablePickaxes.Count}");
        }
        
        shopPanel.SetActive(false);
        PopulateShop();
        UpdateCurrentPickaxe(GameManager.Instance?.CurrentPickaxe);
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPickaxeChanged -= UpdateCurrentPickaxe;
            GameManager.Instance.OnResourcesChanged -= OnResourcesChanged;
        }
    }
    
    private void Update()
    {
        // Press Tab to open/close shop (for testing without player)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab key pressed!");
            ToggleShop();
        }
        
        // Press E to open/close shop when near NPC (for final game)
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed!");
            if (CanAccessShop())
            {
                Debug.Log("Opening shop...");
                ToggleShop();
            }
        }*/
    }
    
    /// <summary>
    /// Check if player can access the shop (near NPC).
    /// </summary>
    private bool CanAccessShop()
    {
        // To implement: check distance to NPC
        // For now, always return true for testing
        return true;
    }
    
    /// <summary>
    /// Toggle shop visibility.
    /// </summary>
    private void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        shopPanel.SetActive(isShopOpen);
        
        // NOUVEAU: Notifier le GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetShopOpen(isShopOpen);
        }
        
        if (isShopOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Don't pause game - it blocks inputs
            // Time.timeScale = 0f;
            RefreshShopItems();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Time.timeScale = 1f;
        }
        
        Debug.Log($"Shop toggled - isOpen: {isShopOpen}");
    }
    
    /// <summary>
    /// Close the shop.
    /// </summary>
    private void CloseShop()
    {
        if (isShopOpen)
        {
            ToggleShop();
        }
    }
    
    /// <summary>
    /// Populate shop with all available pickaxes.
    /// </summary>
    private void PopulateShop()
    {
        if (GameManager.Instance == null || pickaxeItemPrefab == null)
        {
            return;
        }
        
        foreach (var pickaxe in GameManager.Instance.AvailablePickaxes)
        {
            GameObject item = Instantiate(pickaxeItemPrefab, pickaxeListContainer);
            PickaxeItemUI itemUI = item.GetComponent<PickaxeItemUI>();
            
            if (itemUI != null)
            {
                itemUI.Initialize(pickaxe, this);
                pickaxeItems.Add(itemUI);
            }
        }
    }
    
    /// <summary>
    /// Refresh all shop items state.
    /// </summary>
    private void RefreshShopItems()
    {
        foreach (var item in pickaxeItems)
        {
            item.RefreshState();
        }
    }
    
    /// <summary>
    /// Called when resources change.
    /// </summary>
    private void OnResourcesChanged(int coins, int stones)
    {
        RefreshShopItems();
    }
    
    /// <summary>
    /// Update current pickaxe display.
    /// </summary>
    private void UpdateCurrentPickaxe(PickaxeData pickaxe)
    {
        if (pickaxe == null)
        {
            return;
        }
        
        if (currentPickaxeText != null)
        {
            currentPickaxeText.text = $"Current: {pickaxe.PickaxeName}";
        }
        
        if (currentPickaxeIcon != null && pickaxe.PickaxeIcon != null)
        {
            currentPickaxeIcon.sprite = pickaxe.PickaxeIcon;
        }
    }
    
    /// <summary>
    /// Attempt to purchase a pickaxe.
    /// </summary>
    public void PurchasePickaxe(PickaxeData pickaxe)
    {
        if (GameManager.Instance.PurchasePickaxe(pickaxe))
        {
            RefreshShopItems();
            Debug.Log($"Purchased {pickaxe.PickaxeName}");
        }
        else
        {
            Debug.Log("Cannot afford this pickaxe");
        }
    }
    
    /// <summary>
    /// Equip a pickaxe (free if already owned).
    /// </summary>
    public void EquipPickaxe(PickaxeData pickaxe)
    {
        GameManager.Instance.EquipPickaxe(pickaxe);
        RefreshShopItems();
        Debug.Log($"Equipped {pickaxe.PickaxeName}");
    }
    
    /// <summary>
    /// Sell all stones for coins.
    /// Conversion rate: 10 stones = 1 coin
    /// </summary>
    public void SellAllStones()
    {
        if (GameManager.Instance == null) return;
        
        int stonesToSell = GameManager.Instance.Stones;
        
        if (stonesToSell <= 0)
        {
            Debug.Log("No stones to sell!");
            return;
        }
        
        // Conversion rate: 10 stones = 1 coin
        int coinsToReceive = stonesToSell / 10;
        
        // Remove stones and add coins
        GameManager.Instance.AddStones(-stonesToSell);
        GameManager.Instance.AddCoins(coinsToReceive);
        
        Debug.Log($"Sold {stonesToSell} stones for {coinsToReceive} coins!");
    }
}