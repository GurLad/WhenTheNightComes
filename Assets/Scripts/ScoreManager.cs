using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int Points=0, TotalPoints=0, HighScore=0;

    public UnityEngine.UI.Text GameOver, NextLevel, PointCounter;
    void Start()
    {
        UpdateDisplay();
    }

    public void AddPoints(int Points_)
    {
        Points += Points_;
        UpdateDisplay();
    }

    public void EndGame()
    {
        if (HighScore < TotalPoints)
            HighScore = TotalPoints;
        UpdateDisplay();
        TotalPoints = 0;
        Points = 0;
    }

    public void EndLevel()
    {
        TotalPoints += Points;
        UpdateDisplay();
        Points = 0;
    }
    public void UpdateDisplay()
    {
        GameOver.text = "Total Score: " + TotalPoints.ToString() + "\nHighscore: " + HighScore.ToString();
        NextLevel.text = "Score: " + Points.ToString() +"\nTotal Score: " + TotalPoints.ToString() + "\nHighscore: " + HighScore.ToString();
        PointCounter.text = "Score: " + Points.ToString();
    }

    public void SetScores(int points, int totalpoints, int highscore)
    {
        Points = points;
        TotalPoints = totalpoints;
        HighScore = highscore;
        UpdateDisplay();
    }

    public int GetPoints()
    {
        return Points;
    }

    public int GetTotalPoints()
    {
        return TotalPoints;
    }

    public int GetHighScore()
    {
        return HighScore;
    }

}
