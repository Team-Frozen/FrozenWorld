using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayManager : MonoBehaviour
{ 
    public void ChangeScene_Start()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        SceneManager.LoadScene("1_Start");
    }
}
