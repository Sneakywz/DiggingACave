using UnityEngine;

/// <summary>
/// Generates the cave world using different block prefabs.
/// Each block prefab represents a real block type (durability, behavior).
/// NOW SUPPORTS WORLD REGENERATION!
/// </summary>
public class WorldGenerator : MonoBehaviour
{
    [Header("World Size")]
    [SerializeField] private int width = 10;
    [SerializeField] private int depth = 10;
    [SerializeField] private int height = 30;

    [Header("Generation Settings")]
    [SerializeField] private float blockSize = 1f;
    [SerializeField] private int groundLevelY = 0;

    [Header("Block Prefabs")]
    [SerializeField] private GameObject grassBlockPrefab;
    [SerializeField] private GameObject dirtBlockPrefab;
    [SerializeField] private GameObject stoneBlockPrefab;
    [SerializeField] private GameObject rockBlockPrefab;
    [SerializeField] private GameObject bedrockBlockPrefab;

    private void Start()
    {
        GenerateWorld();
    }

    /// <summary>
    /// NEW: Public method to regenerate the entire world
    /// </summary>
    public void RegenerateWorld()
    {
        Debug.Log("Regenerating world...");
        
        // Destroy all existing blocks (children of this GameObject)
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        // Generate new world
        GenerateWorld();
        
        Debug.Log($"World regenerated! Created {transform.childCount} blocks");
    }

    /// <summary>
    /// Generates the full world including borders and bedrock.
    /// </summary>
    private void GenerateWorld()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position = new Vector3(
                        x * blockSize,
                        groundLevelY - (y * blockSize),
                        z * blockSize
                    );

                    GameObject prefabToSpawn = GetBlockPrefabForHeight(y);

                    Instantiate(
                        prefabToSpawn,
                        position,
                        Quaternion.identity,
                        transform
                    );
                }
            }
        }

        GenerateBedrockWalls();
    }

    /// <summary>
    /// Returns the correct block prefab depending on depth.
    /// </summary>
    private GameObject GetBlockPrefabForHeight(int y)
    {
        if (y == 0)
        {
            return grassBlockPrefab;
        }

        if (y <= 3)
        {
            return dirtBlockPrefab;
        }

        if (y <= 10)
        {
            return stoneBlockPrefab;
        }

        if (y < height - 1)
        {
            return rockBlockPrefab;
        }

        return bedrockBlockPrefab;
    }

    /// <summary>
    /// Generates unbreakable bedrock borders around the world.
    /// </summary>
    private void GenerateBedrockWalls()
    {
        for (int x = -1; x <= width; x++)
        {
            for (int z = -1; z <= depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isBorder =
                        x == -1 || x == width ||
                        z == -1 || z == depth ||
                        y == height - 1;

                    if (!isBorder)
                        continue;

                    Vector3 position = new Vector3(
                        x * blockSize,
                        groundLevelY - (y * blockSize),
                        z * blockSize
                    );

                    Instantiate(
                        bedrockBlockPrefab,
                        position,
                        Quaternion.identity,
                        transform
                    );
                }
            }
        }
    }
}