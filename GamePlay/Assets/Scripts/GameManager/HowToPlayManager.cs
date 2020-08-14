using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && Application.platform == RuntimePlatform.Android)
        {
            ChangeScene_Start();
        }
    }
public void ChangeScene_Start()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        SceneManager.LoadScene("1_Start");
    }
}
