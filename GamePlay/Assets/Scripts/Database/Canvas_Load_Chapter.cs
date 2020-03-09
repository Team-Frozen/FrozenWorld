using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Load_Chapter : MonoBehaviour
{
    private static Canvas_Load_Chapter instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
