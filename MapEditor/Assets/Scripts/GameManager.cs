using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string blockType;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (blockType == "org")
            Debug.Log("org");
        else if (blockType == "arw")
            Debug.Log("arw");
    }

    public void setBlockType(string blockType)
    {
        this.blockType = blockType;
    }

    public string getBlockType()
    {
        return blockType;
    }

    public void ChangeScene_MapEdit()
    {
        SceneManager.LoadScene("MapEdit");
    }
    public void ChangeScene_Main()
    {
        SceneManager.LoadScene("Main");
    }
}