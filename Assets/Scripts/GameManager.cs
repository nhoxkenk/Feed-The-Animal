using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
    public bool multiHit = false;
    public int hitCount = 2;
    public float explosionRange = 10.0f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

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
            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    PlayerManager playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
            //    playerManager.ModifyBaseDame(1);
            //    Debug.Log(GameManager.Instance.hitCount);
            //}
        }
        else
        {
            cameraAudio.Stop();
            GameOverScreen.SetActive(true);
            if(score > PersistentData.Instance.HighScore)
            {
                PersistentData.Instance.HighScore = score;
            }
            PersistentData.Instance.SaveScore();
            FinalScore.text = $"Final Score: {score}";
            HighScore.text = $"Final Score: {PersistentData.Instance.HighScore}";
        }     
    }

    public void updateHealthCount()
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
