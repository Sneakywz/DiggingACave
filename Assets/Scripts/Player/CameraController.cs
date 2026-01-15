using UnityEngine;

/// <summary>
/// Classic FPS mouse look.
/// NOW RESPECTS BOTH PAUSE AND SHOP STATE!
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 120f;

    private float xRotation = 0f;
    private Transform player;

    private void Start()
    {
        player = transform.parent;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // CORRECTION: Vérifier que le jeu n'est pas en pause ET que le shop n'est pas ouvert
        if (GameManager.Instance != null && 
            (GameManager.Instance.IsPaused || GameManager.Instance.IsShopOpen))
        {
            return; // Ne rien faire si le jeu est en pause ou si le shop est ouvert
        }

        Look();
    }

    private void Look()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}