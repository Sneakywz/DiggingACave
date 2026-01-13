using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Block Settings")]
    [SerializeField] private int durability = 1;
    [SerializeField] private bool isIndestructible = false;

    /// <summary>
    /// Apply damage to the block.
    /// If durability reaches zero, the block is destroyed.
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
            Destroy(gameObject);
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
}
