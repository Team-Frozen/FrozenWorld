using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    MAIN_BGM,
    GAMEPLAY_BGM,
    BUTTON_SOUND,
    TOGGLE_SOUND,
    GAMECLEAR_SOUND,
    POP_SOUND
}

public class AudioManager : MonoBehaviour
{
    //private bool stop;
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
            }

            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public SoundAudioClip[] AudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public AudioType audioType;
        public AudioClip audioClip;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (SettingData.SoundOn)
            GetComponent<AudioSource>().volume = SettingData.BGMVolume;
        else
            GetComponent<AudioSource>().volume = 0;
    }

    //BGM 틀기
    public void playBGM(AudioType audioType)
    {
        //틀려고 하는 BGM이 현재 BGM과 다른 경우
        if (GetComponent<AudioSource>().clip != GetAudioClip(audioType))
        {
            StartCoroutine(ChangeMusicWithFadeEffect(audioType));
        }
    }

    //효과음 틀기
    public void playSound(AudioType audioType)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        soundGameObject.AddComponent<Sound>();
        if (SettingData.SoundOn)
            audioSource.volume = SettingData.SoundVolume;
        else
            audioSource.volume = 0;

        audioSource.PlayOneShot(GetAudioClip(audioType));
    }

    private IEnumerator ChangeMusicWithFadeEffect(AudioType audioType)
    {
        float t = 0.0f;
        

        for (t = 0; t < 1; t += Time.deltaTime)
        {
            GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume - t * SettingData.BGMVolume;
            yield return null;
        }
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = GetAudioClip(audioType);
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();

        for (t = 0; t < 1; t += Time.deltaTime)
        {
            if(SettingData.SoundOn)
                GetComponent<AudioSource>().volume = t * SettingData.BGMVolume;
            yield return null;
        }
    }

    // audioType과 같은 AudioClip반환
    private AudioClip GetAudioClip(AudioType audioType)
    {
        foreach(SoundAudioClip sounds in AudioClipArray)
            if(sounds.audioType == audioType)
                return sounds.audioClip;
        Debug.LogError("Sound " + audioType + " is not found");
        return null;
    }
}
