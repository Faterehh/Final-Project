using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // For restarting the game

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalScoreText; // Add this for displaying final score

    private int score;
    public float remainingTime = 60f;
    public Color normalColor = Color.green; // Color for normal time
    public Color warningColor = Color.yellow; // Color for the last 10 seconds
    public Color gameOverColor = Color.red; // Color when time is up

    // Add the Game Over Panel and Restart Button references
    public GameObject gameOverPanel;  // Game Over panel
    public Button restartButton;  // Restart button

    // Add Instruction Panel and Start Button references
    public GameObject instructionPanel;  // Instruction panel with start button
    public Button startButton;  // Start button

    private bool isGameOver = false; // Flag to check if the game is over
    private bool isGameStarted = false; // Flag to check if the game has started

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        timerText.color = normalColor; // Set initial color
        gameOverPanel.SetActive(false); // Make sure the Game Over panel is hidden initially
        instructionPanel.SetActive(true); // Show the instruction panel initially
        scoreText.gameObject.SetActive(false); // Hide the score initially
        timerText.gameObject.SetActive(false); // Hide the timer initially

        // Assign button listeners
        startButton.onClick.AddListener(StartGame);  // Add listener to the start button
        restartButton.onClick.AddListener(RestartGame); // Add listener to the restart button

        Time.timeScale = 0; // Pause the game initially

        // Unlock the cursor and make it visible at the start of the game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver || !isGameStarted) return; // If the game is over or hasn't started, skip the rest of the Update

        remainingTime -= Time.deltaTime;

        if (remainingTime > 10)
        {
            timerText.text = "Time: " + Mathf.Ceil(remainingTime).ToString(); // Show whole seconds
            timerText.color = normalColor; // Normal color
        }
        else if (remainingTime >= 0) // Between 0 and 10 seconds
        {
            timerText.text = "Time: " + remainingTime.ToString("F1");
            timerText.color = warningColor; // Change color to warning
        }
        else // When time is up
        {
            remainingTime = 0; // Ensure remaining time doesn't go below zero
            timerText.text = "Time: 0"; // Final display
            timerText.color = gameOverColor; // Change color to red
            GameOver(); // Call the GameOver function
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    // Update the score text UI
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // GameOver function to show the Game Over panel and freeze the game
    public void GameOver()
    {
        isGameOver = true; // Set game over flag
        gameOverPanel.SetActive(true); // Show the Game Over panel
        finalScoreText.text = "Final Score: " + score.ToString(); // Display final score
        finalScoreText.gameObject.SetActive(true); // Make sure final score text is visible

        Time.timeScale = 0; // Freeze the game when it's over

        // Unlock the cursor and make it visible when the game is over
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Function to start the game
    public void StartGame()
    {
        isGameStarted = true; // Set game started flag
        instructionPanel.SetActive(false); // Hide the instruction panel
        scoreText.gameObject.SetActive(true); // Show the score UI
        timerText.gameObject.SetActive(true); // Show the timer UI

        Time.timeScale = 1; // Unfreeze the game

        // Lock the cursor and hide it when the game starts
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Function to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1; // Reset the game speed when restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the current scene
    }
}