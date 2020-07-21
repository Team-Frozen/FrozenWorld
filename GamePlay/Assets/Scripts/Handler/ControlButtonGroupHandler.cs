using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButtonGroupHandler : MonoBehaviour
{
   void Start()
    {
        if (!SettingData.ControlMode_Button)
            gameObject.SetActive(false);
    }
}
