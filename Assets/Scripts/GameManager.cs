using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public TextMeshProUGUI textScore;
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] GameObject PausedScreen;
    [SerializeField] TextMeshProUGUI FinalScore;
    [SerializeField] TextMeshProUGUI HighScore;
    AudioSource cameraAudio;

    PlayerManager playerManager;

    int health = 5;
    int score = 0;
    public bool gameOver = false;
    bool isPause = false;


    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        cameraAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            updateHealthCount();
            updateScore();
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseGame();
            }
        }
        else
        {
            cameraAudio.Stop();
            GameOverScreen.SetActive(true);
            FinalScore.text = $"Final Score: {score}";
            HighScore.text = $"Final Score: {score}";
        }     
    }

    void updateHealthCount()
    {
        int playerHealth = playerManager.returnCurrentHealth();
        if(health != playerHealth && health > 0)
        {
            health = playerHealth;
            textMeshProUGUI.text = health.ToString() + "/5";
        } else if(health == 0)
        {
            gameOver = true;
        }
    }

    void TogglePauseGame()
    {
        if (!isPause)
        {
            isPause = true;
            Time.timeScale = 0f;
            PausedScreen.SetActive(true);
        }
        else
        {
            isPause = false;
            Time.timeScale = 1.0f;
            PausedScreen.SetActive(false);
        }
    }


    void updateScore()
    {
        textScore.text = "Score: " + score.ToString();
    }

    public void setScore(int score)
    {
        this.score += score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#else
        Application.Quit();
#endif
    }
}
