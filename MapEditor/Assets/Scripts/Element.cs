using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public abstract bool inValidArea(Stage stage);
    public abstract void action();
    public abstract BlockType returnType(); //수정수정수정수정

    public bool onBlock(List<GameObject> elements)
    {
        foreach (GameObject element in elements)
        {
            if (transform.position.x == element.transform.position.x &&
                transform.position.z == element.transform.position.z &&
                transform.position.y == element.transform.position.y &&
                element != this.gameObject)
                return true;
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
    public virtual void setVisible()
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
