using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSetting : MonoBehaviour
{
    public void ChangeScene_Start()
    {
        SaveLoadManager.Save_SettingData();
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        SceneManager.LoadScene("1_Start");
    }
}
