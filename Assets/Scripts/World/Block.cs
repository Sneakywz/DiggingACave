using UnityEngine;

/// <summary>
/// Handles block durability and destruction.
/// NOW GIVES REWARDS WHEN DESTROYED!
/// </summary>
public class Block : MonoBehaviour
{
    [SerializeField] private int maxDurability = 1;
    [SerializeField] private int stoneReward = 1; // NEW: Pierres données quand détruit
    [SerializeField] private int coinReward = 0;  // NEW: Coins donnés quand détruit (optionnel)

    private int currentDurability;
    private bool isBedrock;

    private void Awake()
    {
        currentDurability = maxDurability;
    }

    /// <summary>
    /// Applies damage to the block.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isBedrock)
            return;

        currentDurability -= damage;

        if (currentDurability <= 0)
        {
            // NOUVEAU: Donner les récompenses avant de détruire
            GiveRewards();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Give rewards to the player when block is destroyed.
    /// </summary>
    private void GiveRewards()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("Block: Cannot give rewards - GameManager not found!");
            return;
        }

        if (stoneReward > 0)
        {
            GameManager.Instance.AddStones(stoneReward);
            Debug.Log($"Block destroyed! +{stoneReward} stones");
        }

        if (coinReward > 0)
        {
            GameManager.Instance.AddCoins(coinReward);
            Debug.Log($"Block destroyed! +{coinReward} coins");
        }
    }

    /// <summary>
    /// Sets this block as bedrock (unbreakable).
    /// </summary>
    public void SetBedrock()
    {
        isBedrock = true;
        currentDurability = int.MaxValue;
        stoneReward = 0; // Pas de récompense pour le bedrock
        coinReward = 0;
    }

    /// <summary>
    /// Sets durability for a normal block.
    /// </summary>
    public void SetDurability(int durability)
    {
        maxDurability = durability;
        currentDurability = durability;
    }

    /// <summary>
    /// Sets the stone reward for this block.
    /// </summary>
    public void SetStoneReward(int reward)
    {
        stoneReward = reward;
    }

    /// <summary>
    /// Sets the coin reward for this block.
    /// </summary>
    public void SetCoinReward(int reward)
    {
        coinReward = reward;
    }
}