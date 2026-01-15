using UnityEngine;
using TMPro;

/// <summary>
/// Displays player resources (coins and stones) during gameplay.
/// Updates automatically when resources change.
/// Compatible with TextMeshPro.
/// </summary>
public class ResourcesHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI stonesText;
    
    [Header("Display Format")]
    [SerializeField] private string coinsFormat = "Coins: {0}";
    [SerializeField] private string stonesFormat = "Stones: {0}";
    
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnResourcesChanged += UpdateResourceDisplay;
            UpdateResourceDisplay(GameManager.Instance.Coins, GameManager.Instance.Stones);
        }
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnResourcesChanged -= UpdateResourceDisplay;
        }
    }
    
    /// <summary>
    /// Update the displayed resource values.
    /// </summary>
    private void UpdateResourceDisplay(int coins, int stones)
    {
        if (coinsText != null)
        {
            coinsText.text = string.Format(coinsFormat, coins);
        }
        
        if (stonesText != null)
        {
            stonesText.text = string.Format(stonesFormat, stones);
        }
    }
}