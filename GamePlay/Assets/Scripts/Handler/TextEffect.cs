using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(blinkEffect());
    }

    public IEnumerator blinkEffect()
    {
        int i = 0;
        bool change = false;

        while (true)
        {
            if (i == 195)
                change = true;
            else if (i == 0)
                change = false;

            this.GetComponent<Text>().color = new Color(i/255f, i / 255f, i / 255f);
            yield return new WaitForSeconds(0.001f);

            if (change)
                i -= 15;
            else
                i += 15;
        }

    }
}
