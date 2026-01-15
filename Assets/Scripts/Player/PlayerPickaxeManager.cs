using UnityEngine;

/// <summary>
/// Handles pickaxe equipment and provides gameplay values.
/// NOW SUPPORTS MULTIPLE PICKAXE MODELS!
/// </summary>
public class PlayerPickaxeManager : MonoBehaviour
{
    [Header("Current Pickaxe")]
    [SerializeField] private PickaxeData currentPickaxe;

    [Header("Pickaxe Models")]
    [SerializeField] private GameObject woodPickaxeModel;
    [SerializeField] private GameObject stonePickaxeModel;
    [SerializeField] private GameObject ironPickaxeModel;

    private void Start()
    {
        // Hide all pickaxe models FIRST
        HideAllPickaxes();
        
        // Subscribe to GameManager pickaxe changes
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPickaxeChanged += OnPickaxeChanged;
            
            // Initialize with current pickaxe from GameManager
            if (GameManager.Instance.CurrentPickaxe != null)
            {
                EquipPickaxe(GameManager.Instance.CurrentPickaxe);
            }
            else
            {
                // No pickaxe from GameManager, show wood as default
                ShowPickaxe(woodPickaxeModel);
                Debug.Log("GameManager has no pickaxe, showing wood pickaxe by default");
            }
        }
        else
        {
            Debug.LogWarning("PlayerPickaxeManager: GameManager not found!");
            // Show wood pickaxe by default if no GameManager
            ShowPickaxe(woodPickaxeModel);
            Debug.Log("No GameManager found, showing wood pickaxe by default");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPickaxeChanged -= OnPickaxeChanged;
        }
    }

    /// <summary>
    /// Called when the pickaxe changes in GameManager.
    /// </summary>
    private void OnPickaxeChanged(PickaxeData newPickaxe)
    {
        EquipPickaxe(newPickaxe);
    }

    /// <summary>
    /// Equips a new pickaxe and shows the correct model.
    /// </summary>
    public void EquipPickaxe(PickaxeData newPickaxe)
    {
        currentPickaxe = newPickaxe;

        // Hide all pickaxes first
        HideAllPickaxes();

        // Show the correct pickaxe model based on name
        if (newPickaxe != null)
        {
            string pickaxeName = newPickaxe.PickaxeName.ToLower();
            
            if (pickaxeName.Contains("wood"))
            {
                ShowPickaxe(woodPickaxeModel);
                Debug.Log("Equipped Wood Pickaxe model");
            }
            else if (pickaxeName.Contains("stone"))
            {
                ShowPickaxe(stonePickaxeModel);
                Debug.Log("Equipped Stone Pickaxe model");
            }
            else if (pickaxeName.Contains("iron"))
            {
                ShowPickaxe(ironPickaxeModel);
                Debug.Log("Equipped Iron Pickaxe model");
            }
            else
            {
                // Default to wood if name doesn't match
                ShowPickaxe(woodPickaxeModel);
                Debug.LogWarning($"Unknown pickaxe name '{newPickaxe.PickaxeName}', defaulting to wood model");
            }
        }

        Debug.Log($"PlayerPickaxeManager: Equipped {newPickaxe?.PickaxeName ?? "None"}");
    }

    /// <summary>
    /// Hide all pickaxe models.
    /// </summary>
    private void HideAllPickaxes()
    {
        if (woodPickaxeModel != null)
            woodPickaxeModel.SetActive(false);
        
        if (stonePickaxeModel != null)
            stonePickaxeModel.SetActive(false);
        
        if (ironPickaxeModel != null)
            ironPickaxeModel.SetActive(false);
    }

    /// <summary>
    /// Show a specific pickaxe model.
    /// </summary>
    private void ShowPickaxe(GameObject pickaxeModel)
    {
        if (pickaxeModel != null)
        {
            pickaxeModel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Pickaxe model is null! Assign it in the Inspector.");
        }
    }

    /// <summary>
    /// Returns the mining power of the current pickaxe.
    /// </summary>
    public int GetMiningPower()
    {
        return currentPickaxe != null ? currentPickaxe.MiningPower : 0;
    }

    /// <summary>
    /// Compatibility method used by PlayerDig.
    /// </summary>
    public int GetCurrentMiningPower()
    {
        return GetMiningPower();
    }

    /// <summary>
    /// Returns the damage of the current pickaxe.
    /// </summary>
    public int GetDamage()
    {
        return currentPickaxe != null ? currentPickaxe.Damage : 0;
    }

    /// <summary>
    /// Returns the currently equipped pickaxe data.
    /// </summary>
    public PickaxeData GetCurrentPickaxe()
    {
        return currentPickaxe;
    }
}