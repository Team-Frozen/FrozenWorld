using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Text num;
   
    public int getText()
    {
       return Int32.Parse(num.text);
    }

    public void setText(int input)
    {
        num.text = input.ToString();
    }
}
