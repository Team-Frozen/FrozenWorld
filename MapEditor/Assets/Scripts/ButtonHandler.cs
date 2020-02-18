using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Text size;

    public int getStageSize()
    {
       return Int32.Parse(size.text);
    }
}
