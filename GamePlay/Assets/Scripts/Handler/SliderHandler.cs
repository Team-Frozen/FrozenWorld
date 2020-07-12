using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    public enum SliderType
    {
        BGM,
        SOUND
    }

    public SliderType sliderType;
    public GameObject[] Img_SoundOff;

    void Awake()
    {
        if (!SettingData.SoundOn)
        {
            Img_SoundOff[0].SetActive(true);
            Img_SoundOff[1].SetActive(true);
        }
        GetComponent<Slider>().value = Data;
    }

    public void setSliderValue()
    {
        if (Data != GetComponent<Slider>().value && !SettingData.SoundOn)
        {
            SettingData.SoundOn = true;
            Img_SoundOff[0].SetActive(false);
            Img_SoundOff[1].SetActive(false);
        }
        Data = GetComponent<Slider>().value;
    }

    private float Data
    {
        get
        {
            switch (sliderType)
            {
                case SliderType.BGM:
                    return SettingData.BGMVolume;
                case SliderType.SOUND:
                    return SettingData.SoundVolume;
                default:
                    return 0.0f;
            }
        }

        set
        {
            switch (sliderType)
            {
                case SliderType.BGM:
                    SettingData.BGMVolume = value;
                    break;
                case SliderType.SOUND:
                    SettingData.SoundVolume = value;
                    break;
            }
        }
    }
}
