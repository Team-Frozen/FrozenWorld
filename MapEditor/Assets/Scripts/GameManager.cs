using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string blockType;
    public GameObject orgBlock;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << LayerMask.NameToLayer("MouseTarget");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, 100f, layerMask))
        {
            Destroy(GameObject.Find("Cube(Clone)"));
            Instantiate(orgBlock, new Vector3(Mathf.Round(hitInfo.point.x), 1, Mathf.Round(hitInfo.point.z)), Quaternion.identity);
        }
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