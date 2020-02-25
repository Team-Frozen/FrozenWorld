using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerStart : MonoBehaviour
{
    private void Awake()
    {

    }

    public void ChangeScene_Chapters()
    {
        SceneManager.LoadScene("2_Chapters");
    }
}