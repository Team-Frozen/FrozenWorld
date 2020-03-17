using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    void Awake()
    {
        setCameraAngle();
    }

    public void setCameraAngle()
    {
        if (SettingData.CameraAngle_Rectangle)
        {
            transform.position = new Vector3(0, 11, -7);
            transform.rotation = Quaternion.Euler(55, 0, 0);
        }
        else
        {
            transform.position = new Vector3(-9, 11, -9);
            transform.rotation = Quaternion.Euler(40, 45, 0);
        }
    }
}
