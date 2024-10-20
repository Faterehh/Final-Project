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

    public TextMeshProUGUI followingCatsText;  // UI element to show the number of following cats

    private int score;
    public TextMeshProUGUI highScoreText;
    public float remainingTime = 90f;
    public Color normalColor = Color.green; // Color for normal time
    public Color gameOverColor = Color.red; // Color when time is up

    // Add the Game Over Panel and Restart Button references
    public GameObject gameOverPanel;  // Game Over panel
    public Button restartButton;  // Restart button

    // Add Instruction Panel and Start Button references
    public GameObject instructionPanel;  // Instruction panel with start button
    public Button startButton;  // Start button
    public Image compess;

    private bool isGameOver = false; // Flag to check if the game is over
    private bool isGameStarted = false; // Flag to check if the game has started
    
    private bool catsInFastSpeed = false;

    public GameObject plane; // Add this line to reference the plane

    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip lightOffSound; // Reference to the sound effect
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        timerText.color = normalColor; // Set initial color
        gameOverPanel.SetActive(false); // Make sure the Game Over panel is hidden initially
        compess.gameObject.SetActive(false);
        instructionPanel.SetActive(true); // Show the instruction panel initially
        scoreText.gameObject.SetActive(false); // Hide the score initially
        timerText.gameObject.SetActive(false); // Hide the timer initially
        followingCatsText.gameObject.SetActive(false); // Hide the following cats text initially


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
        if(!catsInFastSpeed && remainingTime < 30)
        {
            catsInFastSpeed = true;
            CatMovement[] catList = FindObjectsOfType<CatMovement>();
            foreach (CatMovement cat in catList)
            {
                cat.moveSpeed += 3f; // Increment move speed based on deltaTime
            }
        }

        if (remainingTime > 10)
        {
            timerText.text = "Time: " + Mathf.Ceil(remainingTime).ToString(); // Show whole seconds
            timerText.color = normalColor; // Normal color
        }
        else if (remainingTime >= 0) // Between 0 and 10 seconds
        {
            timerText.text = "Time: " + remainingTime.ToString("F1");
            timerText.color = gameOverColor; // Change color to warning
        }
        else // When time is up
        {
            remainingTime = 0; // Ensure remaining time doesn't go below zero
            timerText.text = "Time: 0"; // Final display
            timerText.color = gameOverColor; // Change color to red
            GameOver(); // Call the GameOver function
        }

        // Start blinking the plane at 25 seconds
        if (remainingTime <= 35f && remainingTime > 34f) // Check if game time is 25 seconds
        {
            StartCoroutine(BlinkPlane());
        }
    }

private IEnumerator BlinkPlane()
{
    // Blink for 12 seconds
    float blinkDuration = 12f;
    float blinkInterval = 1f;
    
    // Play the sound effect when turning off the plane
    PlayLightOffSound(); 

    for (float t = 0; t < blinkDuration; t += blinkInterval)
    {
        // Enable and disable the plane
        plane.SetActive(!plane.activeSelf);
        yield return new WaitForSeconds(blinkInterval);
    }

    // Disable the plane after blinking
    plane.SetActive(false);
    audioSource.Stop();
    
    }

    private void PlayLightOffSound()
    {
        if (audioSource != null && lightOffSound != null)
        {
            audioSource.PlayOneShot(lightOffSound); // Play the sound effect
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

    // Update the number of following cats in the UI
    public void UpdateFollowingCatsText(int count)
    {
        if (followingCatsText != null)
        {
            followingCatsText.text = "Following Cats: " + count.ToString();
        }
    }

    // GameOver function to show the Game Over panel and freeze the game
    public void GameOver()
    {
        isGameOver = true; // Set game over flag
        gameOverPanel.SetActive(true); // Show the Game Over panel
        finalScoreText.text = "Score: " + score.ToString(); // Display final score
        finalScoreText.gameObject.SetActive(true); // Make sure final score text is visible
        int highscore = PlayerPrefs.GetInt("highscore");
      if (score > highscore)
      {
         highscore = score;
         PlayerPrefs.SetInt("highscore", highscore);
         PlayerPrefs.Save();
      }
        highScoreText.text = "Highscore: " + highscore.ToString();


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
        followingCatsText.gameObject.SetActive(true); // Show the following cats text UI
        compess.gameObject.SetActive(true);

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
