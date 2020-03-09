using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    public enum ToggleType
    {
        DISPLAY,
        CAMERA_ANGLE,
        CONTROL
    }

    public GameObject BGImage;
    public GameObject handle;
    public Text text;
    public ToggleType toggleType;

    private bool isOn;
    private bool switching;
    private float t;
    private float moveSpeed;

    void Awake()
    {
        moveSpeed = 3.5f;
        switching = false;
        t = 0.0f;

        isOn = Data;

        setHandle();
    }

    void Update()
    {
        if (switching)
            moveHandle();
    }

    public void Switch()
    {
        if (!switching)
        {
            switching = true;
            AudioManager.Instance.playSound(AudioType.TOGGLE_SOUND);
        }
    }

    private void setHandle()
    {
        if (isOn)
        {
            BGImage.GetComponent<Image>().color = new Color(0, 1, 0);
            handle.transform.localPosition = new Vector3(20, 0, 0);
            text.text = returnText(isOn);
        }
        else
        {
            BGImage.GetComponent<Image>().color = new Color(1, 0, 0);
            handle.transform.localPosition = new Vector3(-20, 0, 0);
            text.text = returnText(isOn);
        }
    }

    private void moveHandle()
    {
        if (isOn)
        {
            BGImage.GetComponent<Image>().color = new Color(t, 1 - t, 0);
            handle.transform.localPosition = new Vector3(Mathf.Lerp(20, -20, t += moveSpeed * Time.deltaTime), 0, 0);
        }
        else
        {
            BGImage.GetComponent<Image>().color = new Color(1 - t, t, 0);
            handle.transform.localPosition = new Vector3(Mathf.Lerp(-20, 20, t += moveSpeed * Time.deltaTime), 0, 0);
        }

        if (t > 1f)
        {
            isOn = !isOn;
            text.text = returnText(isOn);
            Data = isOn;

            switching = false;
            t = 0.0f;
        }
    }

    private bool Data
    {
        get
        {
            switch (toggleType)
            {
                case ToggleType.DISPLAY:
                    return SettingData.Display_Vertical;
                case ToggleType.CAMERA_ANGLE:
                    return SettingData.CameraAngle_Rectangle;
                case ToggleType.CONTROL:
                    return SettingData.ControlMode_Button;
                default:
                    return true;
            }
        }

        set
        {
            switch (toggleType)
            {
                case ToggleType.DISPLAY:
                    SettingData.Display_Vertical = value;
                    break;
                case ToggleType.CAMERA_ANGLE:
                    SettingData.CameraAngle_Rectangle = value;
                    break;
                case ToggleType.CONTROL:
                    SettingData.ControlMode_Button = value;
                    break;
            }
        }
    }

    private string returnText(bool isOn)
    {
        switch (toggleType)
        {
            case ToggleType.DISPLAY:
                if (!isOn)
                    return "Horizontal";
                return "Vertical";
            case ToggleType.CAMERA_ANGLE:
                if (!isOn)
                    return "Rhombus";
                return "Rectangle";
            case ToggleType.CONTROL:
                if (!isOn)
                    return "Swipe";
                return "Button";
            default:
                return "null";
        }
    }
}
