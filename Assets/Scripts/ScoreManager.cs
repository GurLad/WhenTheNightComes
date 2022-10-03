using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _current;
    public static ScoreManager Current
    {
        get
        {
            return _current != null ? _current : (_current = FindObjectOfType<ScoreManager>());
        }
    }
    private int Points=0, TotalPoints=0, HighScore=0, Lifes = 3;

    public GameObject life1, life2, life3;

    public UnityEngine.UI.Text GameOver, NextLevel, PointCounter;

    public bool Playing = true;

    void Start()
    {
        UpdateDisplay();
    }

    public void AddPoints(int Points_) 
    {
        Points += Points_;
        Points = Mathf.Max(Points, 0);
        UpdateDisplay();
    }

    public void LoseLife()
    {
        Lifes--;
        UpdateLifesVisibility();

        if (Lifes == 0) //losing condition. opens game over menu
        {
            EndGame();
            FindObjectOfType<UIManager>().OpenWindow(UIManager.UIElements.GameOver);
        }
    }

    private void UpdateLifesVisibility() //Updates lifes display by changing 3 icon's visibility
    {
        if (Lifes == 3)
        { 
            life1.SetActive(true);
            life2.SetActive(true);
            life3.SetActive(true);
        }
        else if (Lifes == 2)
        {
            life1.SetActive(true);
            life2.SetActive(true);
            life3.SetActive(false);
        }
        else if (Lifes == 1)
        {
            life1.SetActive(true);
            life2.SetActive(false);
            life3.SetActive(false);
        }
        else if (Lifes == 0)
        {
            life1.SetActive(false);
            life2.SetActive(false);
            life3.SetActive(false);
        }

    }

    public void EndGame()
    {
        Conductor.Pause();
        CrossfadeMusicPlayer.Instance.Play("GameOver");
        Playing = false;
        if (HighScore < TotalPoints)
            HighScore = TotalPoints;
        UpdateDisplay();
        TotalPoints = 0;
        Points = 0;
    }

    public void EndLevel() // Win
    {
        Conductor.Pause();
        CrossfadeMusicPlayer.Instance.Play("Menu");
        Playing = false;
        TotalPoints += Points;
        UpdateDisplay();
        Lifes = 3;
        UpdateLifesVisibility();
        Points = 0;
    }
    public void UpdateDisplay()  
    {
        //GameOver.text = "Total Score: " + TotalPoints.ToString() + "\nHighscore: " + HighScore.ToString();
        //NextLevel.text = "Score: " + Points.ToString() +"\nTotal Score: " + TotalPoints.ToString() + "\nHighscore: " + HighScore.ToString();
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
