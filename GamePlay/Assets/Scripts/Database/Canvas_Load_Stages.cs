using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//수정 조건 수정
public class Canvas_Load_Stages : MonoBehaviour 
{
    void Awake()
    {
       DontDestroyOnLoad(gameObject);
    }
}
