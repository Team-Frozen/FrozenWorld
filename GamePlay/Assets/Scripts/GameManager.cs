using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int currentScene;
    
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        //DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        currentScene = buildIndex;
    }

    public void SelectStage(int selectedBtn)
    {
        SceneManager.LoadScene("Stage");
        StageManager.currentStage = selectedBtn;
    }
}
