using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;
    private Vector3 desiredPosition;
    private bool camera_zoom;
    private float desiredSize;

    void Awake()
    {
        camera_zoom = true;
        SettingData.Camera_Zoom = true;
        GetComponent<Camera>().orthographicSize = 5;
        desiredSize = 5;

        target = Database.Player.GetComponent<Transform>();
        setCameraAngle();
    }
    void Update()
    {
        if (camera_zoom != SettingData.Camera_Zoom)
        {
            camera_zoom = SettingData.Camera_Zoom;
            desiredSize = camera_zoom ? 5 : 11;
            setCameraAngle();
        }
    }
    void LateUpdate()
    {
        if(camera_zoom)
            desiredPosition = target.position + offset;
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, desiredSize, 0.125f);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.125f);
    }

    public void setCameraAngle()
    {
        if (SettingData.CameraAngle_Rectangle)
        {
            offset = new Vector3(0, 9, -7);
            desiredPosition = new Vector3(0, 10, -7);
            transform.rotation = Quaternion.Euler(55, 0, 0);
        }
        else
        {
            offset = new Vector3(-9, 14, -9);
            desiredPosition = new Vector3(-9, 15, -9);
            transform.rotation = Quaternion.Euler(50, 45, 0);
        }
    }
}
