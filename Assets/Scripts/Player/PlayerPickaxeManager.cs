using UnityEngine;

/// <summary>
/// Handles pickaxe equipment and provides gameplay values.
/// </summary>
public class PlayerPickaxeManager : MonoBehaviour
{
    [Header("Current Pickaxe")]
    [SerializeField] private PickaxeData currentPickaxe;

    [Header("Visual")]
    [SerializeField] private GameObject pickaxeVisual;

    /// <summary>
    /// Equips a new pickaxe.
    /// </summary>
    public void EquipPickaxe(PickaxeData newPickaxe)
    {
        currentPickaxe = newPickaxe;

        if (pickaxeVisual != null)
        {
            pickaxeVisual.SetActive(true);
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
