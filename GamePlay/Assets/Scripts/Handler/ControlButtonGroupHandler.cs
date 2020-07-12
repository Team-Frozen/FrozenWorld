using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButtonGroupHandler : MonoBehaviour
{
   void Start()
    {
        if (!SettingData.ControlMode_Button)
            gameObject.SetActive(false);
        if (SettingData.CameraAngle_Rectangle)
            transform.rotation = Quaternion.Euler(0, 0, -45);
    }
}
