using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerChapters : MonoBehaviour
{
    public void ChangeScene_Stages()
    {
        //추가
        SceneManager.LoadScene("3_Stages");
    }

    public void ChangeScene_Start()
    {
        SceneManager.LoadScene("1_Start");
    }
}
