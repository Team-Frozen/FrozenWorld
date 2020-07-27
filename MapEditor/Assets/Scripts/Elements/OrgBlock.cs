using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrgBlock : Block
{
    public override void changeProperty()
    {
        setProperty((property + 1) % 2);
    }

    public override void setProperty(int property)
    {
        this.property = property;
        if (property == 1)
            this.transform.GetComponent<MeshRenderer>().material.color = Color.red;
        else
            this.transform.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public override BlockType returnType()
    {
        return BlockType.ORG;
    }
}
