using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerStart : MonoBehaviour
{
    public GameObject settingPanel;

    void Awake()
    {
        AudioManager.Instance.playBGM(AudioType.MAIN_BGM);
    }

    public void ChangeScene_Chapters()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        SceneManager.LoadScene("2_Chapters");
    }

    public void ShowSettingPanel()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        SaveLoadManager.Save_SettingData();
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        settingPanel.SetActive(false);
    }
    public void ChangeScene_Character()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        SceneManager.LoadScene("1-1_Character");
    }
}