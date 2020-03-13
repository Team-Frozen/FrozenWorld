using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    void Awake()
    {
        if (SettingData.CameraAngle_Rectangle)
        {
            transform.position = new Vector3(0, 11, -8);
            transform.rotation = Quaternion.Euler(55, 0, 0);
        }
    }
}
