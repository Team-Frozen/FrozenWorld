using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);    
    }
    void Update()
    {
        if(GetComponent<AudioSource>().isPlaying == false)
            Destroy(this.gameObject);
    }
}
