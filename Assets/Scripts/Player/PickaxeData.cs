using UnityEngine;

/// <summary>
/// Stores all static data related to a pickaxe.
/// Shared between gameplay and UI.
/// </summary>
[CreateAssetMenu(fileName = "PickaxeData", menuName = "Data/Pickaxe")]
public class PickaxeData : ScriptableObject
{
    [Header("Gameplay")]
    [SerializeField] private int damage;
    [SerializeField] private int miningPower;

    [Header("UI")]
    [SerializeField] private string pickaxeName;
    [SerializeField] private Sprite pickaxeIcon;

    [Header("Shop Prices")]
    [SerializeField] private int coinPrice;
    [SerializeField] private int stonePrice;

    /// <summary>
    /// Gets the damage dealt by the pickaxe.
    /// </summary>
    public int Damage => damage;

    /// <summary>
    /// Gets the mining power of the pickaxe.
    /// </summary>
    public int MiningPower => miningPower;

    /// <summary>
    /// Gets the display name of the pickaxe.
    /// </summary>
    public string PickaxeName => pickaxeName;

    /// <summary>
    /// Gets the icon used in the UI.
    /// </summary>
    public Sprite PickaxeIcon => pickaxeIcon;

    /// <summary>
    /// Gets the coin price.
    /// </summary>
    public int CoinPrice => coinPrice;

    /// <summary>
    /// Gets the stone price.
    /// </summary>
    public int StonePrice => stonePrice;
}
