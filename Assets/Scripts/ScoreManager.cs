using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private const int MAX_SCORE = 99999;

    private static float currentScore;

    public static Text scoreText;

    void Start () {
        currentScore = 0;
        UpdateScore();
    }
	
    public static void AddScore(int scoreToAdd)
    {
        if (!HasReachedMaxScore(scoreToAdd))
        {
            currentScore += scoreToAdd;
        } else
        {
            currentScore = MAX_SCORE;
        }

        UpdateScore();
    }

    private static bool HasReachedMaxScore(int scoreToAdd)
    {
        return (currentScore + scoreToAdd) >= MAX_SCORE;
    }

    private static void UpdateScore()
    {
        scoreText.text = "SCORE: " + currentScore.ToString();
    }

}
