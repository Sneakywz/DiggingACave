using UnityEngine;

/// <summary>
/// Represents a mineable block that can drop resources.
/// </summary>
public class Block : MonoBehaviour
{
    [Header("Block Settings")]
    [SerializeField] private int durability = 1;
    [SerializeField] private bool isIndestructible = false;
    
    [Header("Loot")]
    [SerializeField] private int coinsOnDestroy = 0;
    [SerializeField] private int stonesOnDestroy = 1;
    [SerializeField] private float lootDropChance = 1f;
    
    /// <summary>
    /// Apply damage to the block.
    /// If durability reaches zero, the block is destroyed and drops loot.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isIndestructible)
        {
            return;
        }
        
        durability -= damage;
        
        if (durability <= 0)
        {
            DropLoot();
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Drop resources when block is destroyed.
    /// </summary>
    private void DropLoot()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        
        if (Random.value <= lootDropChance)
        {
            if (coinsOnDestroy > 0)
            {
                GameManager.Instance.AddCoins(coinsOnDestroy);
            }
            
            if (stonesOnDestroy > 0)
            {
                GameManager.Instance.AddStones(stonesOnDestroy);
            }
        }
    }
    
    /// <summary>
    /// Sets the durability of the block.
    /// </summary>
    public void SetDurability(int value)
    {
        durability = Mathf.Max(1, value);
    }
    
    /// <summary>
    /// Makes the block indestructible (used for bedrock).
    /// </summary>
    public void SetIndestructible()
    {
        isIndestructible = true;
    }
    
    /// <summary>
    /// Set the loot this block drops.
    /// </summary>
    public void SetLoot(int coins, int stones, float dropChance = 1f)
    {
        coinsOnDestroy = coins;
        stonesOnDestroy = stones;
        lootDropChance = dropChance;
    }
}
