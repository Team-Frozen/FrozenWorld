using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentStage;
    public int playerMoves;
    public int minMoves;
    public Text movesText;
    
    void Awake()
    {
        movesText.text = "0";
    }
    
    void Update()
    {
        movesText.text = playerMoves.ToString();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(currentStage + 1);
    }
}
