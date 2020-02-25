using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerStages : MonoBehaviour
{
    public void ChangeScene_GamePlay(int selectedBtn)
    {
        //추가

        Database.FocusStage = selectedBtn;
        SceneManager.LoadScene("4_GamePlay");
    }

    public void ChangeScene_Chapters()
    {
        SceneManager.LoadScene("2_Chapters");
    }
}
