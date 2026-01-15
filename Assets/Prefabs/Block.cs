using UnityEngine;

/// <summary>
/// Handles block durability and destruction.
/// </summary>
public class Block : MonoBehaviour
{
    [SerializeField] private int maxDurability = 1;

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
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets this block as bedrock (unbreakable).
    /// </summary>
    public void SetBedrock()
    {
        isBedrock = true;
        currentDurability = int.MaxValue;
    }

    /// <summary>
    /// Sets durability for a normal block.
    /// </summary>
    public void SetDurability(int durability)
    {
        maxDurability = durability;
        currentDurability = durability;
    }
}