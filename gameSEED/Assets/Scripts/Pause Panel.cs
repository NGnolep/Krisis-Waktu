using UnityEngine;
using UnityEngine.SceneManagement; // For quitting to the main menu
using UnityEngine.UI; // For UI elements

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;  // Reference to the pause panel UI
    public Button resumeButton;    // Reference to the Resume button
    public Button quitButton;      // Reference to the Quit button
    private bool isPaused = false; // Track if the game is paused

    void Start()
    {
        // Ensure the pause panel is hidden at the start
        pausePanel.SetActive(false);

        // Add listeners for the buttons
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitToMainMenu);
    }

    void Update()
    {
        // Check if Escape key is pressed to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Function to pause the game
    void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true); // Show the pause panel
        Time.timeScale = 0f; // Freeze the game
    }

    // Function to resume the game
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); // Hide the pause panel
        Time.timeScale = 1f; // Resume the game
    }

    // Function to quit to the main menu
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Ensure the game isn't paused in the main menu
        SceneManager.LoadScene(0); // Load the MainMenu scene (using scene index -1)
    }
}
