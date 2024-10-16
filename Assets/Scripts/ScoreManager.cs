using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{   
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    private int score;
    public float remainingTime = 120f;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        timerText.text = "Time: " + remainingTime.ToString("F1");

        if(remainingTime < 0)
        {
            //TODO: Game Over!!
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
}
