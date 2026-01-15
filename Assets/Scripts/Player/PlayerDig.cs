using UnityEngine;

/// <summary>
/// Handles block mining using raycasting from the player camera.
/// NOW RESPECTS BOTH PAUSE AND SHOP STATE!
/// </summary>
public class PlayerDig : MonoBehaviour
{
    [SerializeField] private float digRange = 3f;

    private Camera playerCamera;
    private PlayerPickaxeManager pickaxeManager;

    private void Awake()
    {
        // Get the camera from children (PlayerCamera)
        playerCamera = GetComponentInChildren<Camera>();

        // Get pickaxe manager
        pickaxeManager = GetComponent<PlayerPickaxeManager>();
    }

    private void Update()
    {
        // CORRECTION: Vérifier que le jeu n'est pas en pause ET que le shop n'est pas ouvert
        if (GameManager.Instance != null && 
            (GameManager.Instance.IsPaused || GameManager.Instance.IsShopOpen))
        {
            return; // Ne rien faire si le jeu est en pause ou si le shop est ouvert
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryDig();
        }
    }

    private void TryDig()
    {
        if (playerCamera == null || pickaxeManager == null)
            return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, digRange))
        {
            Block block = hit.collider.GetComponent<Block>();

            if (block == null)
                return;

            int damage = pickaxeManager.GetCurrentMiningPower();
            block.TakeDamage(damage);
        }
    }
}