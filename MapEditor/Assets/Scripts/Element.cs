using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool inValidArea(Stage stage)
    {
        List<GameObject> elements = stage.getElements();
        int size = stage.getStageSize();
        if (inStage(size) && !onBlock(elements))  //Stage 안에 있고 블럭이 없는 곳
            return true;
        return false;
    }

    public bool onBlock(List<GameObject> elements)
    {
        foreach (GameObject element in elements)
        {
            if (transform.position.x == element.transform.position.x &&
                transform.position.z == element.transform.position.z &&
                transform.position.y == element.transform.position.y &&
                element != this.gameObject)
            {
                return true;
            }
        }
        return false;
    }
   
    public bool inStage(int size)
    {
        if (transform.position.x < 0.5f * size &&
            transform.position.x > -0.5f * size &&
            transform.position.z < 0.5f * size &&
            transform.position.z > -0.5f * size)
            return true;
        return false;
    }
    public void setVisible()
    {
        gameObject.layer = 8;
        GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    public void setInvisible()
    {
        gameObject.layer = 0;
        GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
