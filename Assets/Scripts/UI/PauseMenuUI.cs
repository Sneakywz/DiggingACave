using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button respawnButton;
    [SerializeField] private Button resetWorldButton;
    [SerializeField] private Button resetGameButton;
    [SerializeField] private Button leaveButton;
    
    private void Start()
    {
        SetupButtons();
        GameManager.Instance.OnGamePaused += OnGamePausedChanged;
        pauseMenuPanel.SetActive(false);
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePaused -= OnGamePausedChanged;
        }
    }
    
    private void SetupButtons()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);
        
        if (respawnButton != null)
            respawnButton.onClick.AddListener(Respawn);
        
        if (resetWorldButton != null)
            resetWorldButton.onClick.AddListener(ResetWorld);
        
        if (resetGameButton != null)
            resetGameButton.onClick.AddListener(ResetGame);
        
        if (leaveButton != null)
            leaveButton.onClick.AddListener(Leave);
    }
    
    private void OnGamePausedChanged(bool paused)
    {
        pauseMenuPanel.SetActive(paused);
    }
    
    private void Resume()
    {
        GameManager.Instance.TogglePause();
    }
    
    private void Respawn()
    {
        GameManager.Instance.RespawnPlayer();
    }
    
    private void ResetWorld()
    {
        GameManager.Instance.ResetWorld();
        Resume();
    }
    
    private void ResetGame()
    {
        GameManager.Instance.ResetGame();
        Resume();
    }
    
    private void Leave()
    {
        Resume();
        GameManager.Instance.QuitToMenu();
    }
}