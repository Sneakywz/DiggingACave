using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles main menu UI interactions.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameSceneName = "GameScene";
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }
    
    /// <summary>
    /// Load the game scene.
    /// </summary>
    private void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
    /// <summary>
    /// Quit the application.
    /// </summary>
    private void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
