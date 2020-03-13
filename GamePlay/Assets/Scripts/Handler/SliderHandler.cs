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

    void Awake()
    {
        GetComponent<Slider>().value = Data;
    }

    public void setSliderValue()
    {
        GetComponent<Slider>().value = Mathf.Floor(GetComponent<Slider>().value * 4 + 0.5f) * 0.25f;

        if (GetComponent<Slider>().value != Data)
        {
            Data = GetComponent<Slider>().value;
            AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        }
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
