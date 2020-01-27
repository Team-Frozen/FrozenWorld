using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string blockType;
    private GameObject ghostBlock;  //
    public GameObject orgGhost;     //Prefab
    public GameObject orgBlock;     //Prefab

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ghostBlock != null)
        {
            Instantiate(orgBlock, ghostBlock.transform.position, Quaternion.identity);
            Destroy(ghostBlock);
        }

        int layerMask = 1 << LayerMask.NameToLayer("MouseTarget");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100f, layerMask))
        {
            if (ghostBlock == null)
                ghostBlock = Instantiate(orgGhost, new Vector3(Mathf.Round(hitInfo.point.x), 1, Mathf.Round(hitInfo.point.z)), Quaternion.identity);
            else
                ghostBlock.transform.position = new Vector3(Mathf.Round(hitInfo.point.x), 1, Mathf.Round(hitInfo.point.z));
        }
        else
            Destroy(ghostBlock);


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