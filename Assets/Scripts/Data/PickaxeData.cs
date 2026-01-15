using UnityEngine;

/// <summary>
/// ScriptableObject containing pickaxe information.
/// Used for shop and inventory management.
/// </summary>
[CreateAssetMenu(fileName = "NewPickaxe", menuName = "DiggingACave/Pickaxe Data")]
public class PickaxeData : ScriptableObject
{
    [Header("Pickaxe Info")]
    [SerializeField] private string pickaxeName = "Wooden Pickaxe";
    [SerializeField] private Sprite pickaxeIcon;
    [SerializeField] private int digDamage = 1;
    [SerializeField] private int stonePrice = 0;
    [SerializeField] private int coinPrice = 0;
    
    [Header("Visual")]
    [SerializeField] private GameObject pickaxePrefab;
    
    public string PickaxeName => pickaxeName;
    public Sprite PickaxeIcon => pickaxeIcon;
    public int DigDamage => digDamage;
    public int StonePrice => stonePrice;
    public int CoinPrice => coinPrice;
    public GameObject PickaxePrefab => pickaxePrefab;
}
