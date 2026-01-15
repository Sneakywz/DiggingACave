using UnityEngine;
using UnityEngine.UI;
using TMPro; 

/// <summary>
/// Represents a single pickaxe item in the shop.
/// Shows name, icon, price, and allows purchase/equip.
/// </summary>
public class PickaxeItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image pickaxeIcon;
    [SerializeField] private TextMeshProUGUI pickaxeNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    
    [Header("State Colors")]
    [SerializeField] private Color ownedColor = Color.green;
    [SerializeField] private Color equippedColor = Color.yellow;
    [SerializeField] private Color canAffordColor = Color.white;
    [SerializeField] private Color cannotAffordColor = Color.red;
    
    private PickaxeData pickaxeData;
    private ShopUI shopUI;
    
    /// <summary>
    /// Initialize the pickaxe item with data.
    /// </summary>
    public void Initialize(PickaxeData data, ShopUI shop)
    {
        pickaxeData = data;
        shopUI = shop;
        
        if (pickaxeIcon != null && data.PickaxeIcon != null)
        {
            pickaxeIcon.sprite = data.PickaxeIcon;
        }
        
        if (pickaxeNameText != null)
        {
            pickaxeNameText.text = data.PickaxeName;
        }
        
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnButtonClicked);
        }
        
        RefreshState();
    }
    
    /// <summary>
    /// Refresh the item state based on ownership and affordability.
    /// </summary>
    public void RefreshState()
    {
        if (pickaxeData == null || GameManager.Instance == null)
        {
            return;
        }
        
        bool isOwned = GameManager.Instance.OwnsPickaxe(pickaxeData);
        bool isEquipped = GameManager.Instance.CurrentPickaxe == pickaxeData;
        bool canAfford = GameManager.Instance.Coins >= pickaxeData.CoinPrice && 
                        GameManager.Instance.Stones >= pickaxeData.StonePrice;
        
        // Update price display
        if (priceText != null)
        {
            if (isOwned)
            {
                priceText.text = "Owned";
                priceText.color = ownedColor;
            }
            else
            {
                string price = "";
                if (pickaxeData.CoinPrice > 0)
                {
                    price += $"{pickaxeData.CoinPrice} Coins";
                }
                if (pickaxeData.StonePrice > 0)
                {
                    if (price.Length > 0) price += " + ";
                    price += $"{pickaxeData.StonePrice} Stones";
                }
                if (price.Length == 0)
                {
                    price = "Free";
                }
                
                priceText.text = price;
                priceText.color = canAfford ? canAffordColor : cannotAffordColor;
            }
        }
        
        // Update button
        if (actionButton != null && buttonText != null)
        {
            if (isEquipped)
            {
                buttonText.text = "Equipped";
                buttonText.color = equippedColor;
                actionButton.interactable = false;
            }
            else if (isOwned)
            {
                buttonText.text = "Equip";
                buttonText.color = ownedColor;
                actionButton.interactable = true;
            }
            else
            {
                buttonText.text = "Buy";
                buttonText.color = canAfford ? canAffordColor : cannotAffordColor;
                actionButton.interactable = canAfford;
            }
        }
    }
    
    /// <summary>
    /// Handle button click (buy or equip).
    /// </summary>
    private void OnButtonClicked()
    {
        if (pickaxeData == null || shopUI == null)
        {
            return;
        }
        
        bool isOwned = GameManager.Instance.OwnsPickaxe(pickaxeData);
        
        if (isOwned)
        {
            shopUI.EquipPickaxe(pickaxeData);
        }
        else
        {
            shopUI.PurchasePickaxe(pickaxeData);
        }
    }
}
