using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum BlockType
    {
        NULL,
        ORG,
        ARW,
        PORT
    }
    private BlockType blockType;
    private List<GameObject> elements = new List<GameObject>(); //수정 <GameObject>말고 <Element>로 만들 수 있는지 생각해보기
    private GameObject ghostBlock;  //Block before click
    private GameObject focusBlock;  //Clicked block

//----------- Prefabs-------------//
    public GameObject orgGhost;   //
    public GameObject orgBlock;   //
    public GameObject arwGhost;   //
    public GameObject arwBlock;   //
//--------------------------------//

    public ToggleGroup ToggleGroup;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        getSelectedBlockType();
    }
    void OnGUI()
    {
        Event m_Event = Event.current;
        int layerMask = 1 << LayerMask.NameToLayer("MouseTarget");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 100f, layerMask);

//--------마우스 클릭 (맵 안에서)----------//
        if (m_Event.type == EventType.MouseDown && inStage(hitInfo)) 
        {
            //블럭을 클릭한 경우와 그렇지 않은 경우
            if (onBlock(hitInfo))
            {
                focusBlock = hitInfo.transform.gameObject;
            }
            else
            {
                switch (blockType)
                {
                    case BlockType.ORG:
                        elements.Add(Instantiate(orgBlock, ghostBlock.transform.position, Quaternion.identity));
                        break;
                    case BlockType.ARW:
                        elements.Add(Instantiate(arwBlock, ghostBlock.transform.position, Quaternion.identity));
                        break;
                }
                Destroy(ghostBlock);
                focusBlock = null;
            }
        }
//-----------------------------------------//

//-------------마우스 드래그---------------//
        if (m_Event.type == EventType.MouseDrag)
        {
           if(focusBlock != null)
            {
                focusBlock.transform.position = new Vector3(Mathf.Round(hitInfo.point.x), 1, Mathf.Round(hitInfo.point.z));

                if (inStage(hitInfo)) //수정 Element.cs 만들고 안에 function으로 inValidArea 만들고 넣기
                    focusBlock.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                else
                    focusBlock.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0f);
            }
        }
//-----------------------------------------//

//--------------마우스 놓음----------------//
        if (m_Event.type == EventType.MouseUp)
        {
            focusBlock = null;
        }
//-----------------------------------------//

//--------------마우스 이동----------------//
        if (inValidArea(hitInfo))
        {
            if (ghostBlock == null)
            {
                switch (blockType)
                {
                    case BlockType.ORG:
                        ghostBlock = Instantiate(orgGhost, new Vector3(Mathf.Round(hitInfo.point.x), 1, Mathf.Round(hitInfo.point.z)), Quaternion.identity);
                        break;
                    case BlockType.ARW:
                        ghostBlock = Instantiate(arwGhost, new Vector3(Mathf.Round(hitInfo.point.x), 1, Mathf.Round(hitInfo.point.z)), Quaternion.identity);
                        break;
                }
            }
            else
                ghostBlock.GetComponent<GhostBlock>().move(hitInfo);
        }
        else
            Destroy(ghostBlock);
//-----------------------------------------//
    }

    public void getSelectedBlockType()
    {
        Toggle selectedToggle = ToggleGroup.ActiveToggles().FirstOrDefault();
        if (selectedToggle != null)
            blockType = (BlockType)selectedToggle.GetComponent<ToggleHandler>().getBlockType();
        else
            blockType = 0;
    }

    public bool inValidArea(RaycastHit hitInfo)
    {
        if (inStage(hitInfo) && !onBlock(hitInfo))  //Stage 안에 있고 블럭이 없는 곳
            return true;
        return false;
    }

    public bool onBlock(RaycastHit hitInfo)
    {
       foreach (GameObject element in elements)
        {
            if (hitInfo.transform.position.x == element.transform.position.x &&
                hitInfo.transform.position.z == element.transform.position.z &&
                hitInfo.transform.position.y == element.transform.position.y)
            {
                return true;
            }
        }
        return false;
    }

    public bool inStage(RaycastHit hitInfo)
    {
        if (hitInfo.point.x <  4.5 &&
            hitInfo.point.x > -4.5 &&
            hitInfo.point.z <  4.5 &&
            hitInfo.point.z > -4.5)
            return true;
        return false;
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