using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    protected int property;

    public abstract bool inValidArea(Stage stage);
    public abstract void deleteElement();
    public abstract BlockType returnType();
    public abstract void changeProperty();

    protected bool onBlock(List<GameObject> elements)
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
   
    protected bool inStage(int size)
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
        gameObject.SetActive(true);

    }
    public virtual void setInvisible()
    {
        gameObject.layer = 0;
        gameObject.SetActive(false);
    }

    public int getProperty()
    {
        return property;
    }
    public virtual void setProperty(int property)
    {
        this.property = property;
    }
}
