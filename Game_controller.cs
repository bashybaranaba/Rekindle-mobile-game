using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();

    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject StartPage;
    public GameObject GameOverPage;
    public GameObject HighScorePage;
    public Text scoreText;

    enum PageState
    {
        None,
        Start,
        GameOver,
        HighScore
    }

    int score = 0;
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } }

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
    }

    void OnDisable()
    {
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }

    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("highScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("highScore", score);
            SetPageState(PageState.HighScore);
        }
        else
        {
            SetPageState(PageState.GameOver);
        }
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void SetPageState(PageState state)
    {
        switch(state)
        {
            case PageState.None:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                HighScorePage.SetActive(false);
                break;

            case PageState.Start:
                StartPage.SetActive(true);
                GameOverPage.SetActive(false);
                HighScorePage.SetActive(false);
                break;

            case PageState.GameOver:
                StartPage.SetActive(false);
                GameOverPage.SetActive(true);
                HighScorePage.SetActive(false);
                break;

            case PageState.HighScore:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                HighScorePage.SetActive(true);
                break;
        }

        
    }

    void Start()
    {
        OnGameOverConfirmed();
    }
 
    public void ConfirmGameOver()
    {
        //activated when replay button is pressed
        OnGameOverConfirmed();// event sent to TapController
        scoreText.text = "0";
        SetPageState(PageState.Start);

    }
    public void StartGame()
    {
        //activated when play button is pressed
        OnGameStarted();// event sent to TapController
        score = 0;
        gameOver = false;
        SetPageState(PageState.None);
    }

}
