using UnityEngine;

/// <summary>
/// Temporary script to test block damage with mouse clicks.
/// </summary>
public class BlockTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Block block = hit.collider.GetComponent<Block>();

                if (block != null)
                {
                    block.TakeDamage(1);
                }
            }
        }
    }
}
