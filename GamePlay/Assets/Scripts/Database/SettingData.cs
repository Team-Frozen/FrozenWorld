using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingData
{
    private static float backgroundMusicVolume;
    private static float soundVolume;
    private static bool cameraAngle_Rectangle;
    private static bool display_Vertical;
    private static bool controlMode_Button;

    public static float BGMVolume
    {
        get
        {
            return backgroundMusicVolume;
        }
        set
        {
            backgroundMusicVolume = value;
        }
    }

    public static float SoundVolume
    {
        get
        {
            return soundVolume;
        }
        set
        {
            soundVolume = value;
        }
    }

    public static bool CameraAngle_Rectangle
    {
        get
        {
            return cameraAngle_Rectangle;
        }
        set
        {
            cameraAngle_Rectangle = value;
        }
    }

    public static bool Display_Vertical
    {
        get
        {
            return display_Vertical;
        }
        set
        {
            display_Vertical = value;
        }
    }

    public static bool ControlMode_Button
    {
        get
        {
            return controlMode_Button;
        }
        set
        {
            controlMode_Button = value;
        }
    }
}
