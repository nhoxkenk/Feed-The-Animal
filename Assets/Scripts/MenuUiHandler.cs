using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUiHandler : MonoBehaviour
{
    [SerializeField] GameObject helpScreen;
    [SerializeField] GameObject MainScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HelperScreenActive()
    {
        helpScreen.SetActive(true);
        MainScreen.SetActive(false);
    }

    public void MainScreenActive()
    {
        helpScreen.SetActive(false);
        MainScreen.SetActive(true);
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
