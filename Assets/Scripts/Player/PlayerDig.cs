using UnityEngine;

/// <summary>
/// Handles block mining using raycasting.
/// </summary>
public class PlayerDig : MonoBehaviour
{
    [SerializeField] private float digRange = 3f;
    [SerializeField] private int digDamage = 1;

    private Camera cam;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, digRange))
            {
                Block block = hit.collider.GetComponent<Block>();
                if (block != null)
                {
                    block.TakeDamage(digDamage);
                }
            }
        }
    }
}
