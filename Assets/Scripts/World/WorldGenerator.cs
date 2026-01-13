using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("World Size")]
    public int width = 10;
    public int height = 30;
    public int depth = 10;

    [Header("Prefabs")]
    public GameObject blockPrefab;

    [Header("Materials")]
    public Material grassMaterial;
    public Material dirtMaterial;
    public Material stoneMaterial;
    public Material rockMaterial;
    public Material bedrockMaterial;

    private void Start()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Vector3 position = transform.position + new Vector3(x, -y, z);
                    GameObject blockObj = Instantiate(blockPrefab, position, Quaternion.identity, transform);

                    Block block = blockObj.GetComponent<Block>();
                    MeshRenderer renderer = blockObj.GetComponent<MeshRenderer>();

                    bool isBorder =
                        x == 0 || x == width - 1 ||
                        z == 0 || z == depth - 1 ||
                        y == height - 1;

                    if (isBorder)
                    {
                        renderer.material = bedrockMaterial;
                        block.SetIndestructible();
                        continue;
                    }

                    // Surface
                    if (y == 0)
                    {
                        renderer.material = grassMaterial;
                        block.SetDurability(1);
                    }
                    else if (y <= 5)
                    {
                        renderer.material = dirtMaterial;
                        block.SetDurability(2);
                    }
                    else if (y <= 8)
                    {
                        renderer.material = Random.value < 0.5f ? dirtMaterial : stoneMaterial;
                        block.SetDurability(3);
                    }
                    else if (y <= 15)
                    {
                        renderer.material = stoneMaterial;
                        block.SetDurability(4);
                    }
                    else if (y <= 18)
                    {
                        renderer.material = Random.value < 0.5f ? stoneMaterial : rockMaterial;
                        block.SetDurability(5);
                    }
                    else
                    {
                        renderer.material = rockMaterial;
                        block.SetDurability(6);
                    }
                }
            }
        }
    }
}
