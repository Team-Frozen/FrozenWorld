using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingData
{
    private static float backgroundMusicVolume;
    private static float soundVolume;
    private static bool cameraAngle_Rectangle;
    private static bool camera_zoom;
    private static bool display_Vertical;
    private static bool controlMode_Button;
    private static bool soundOn;

    public static bool SoundOn
    {
        get
        {
            return soundOn;
        }
        set
        {
            soundOn = value;
        }
    }

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

    public static bool Camera_Zoom
    {
        get
        {
            return camera_zoom;
        }
        set
        {
            camera_zoom = value;
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
