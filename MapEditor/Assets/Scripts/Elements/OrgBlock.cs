using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrgBlock : Block
{
    public override void changeProperty()
    {
        setProperty((property + 1) % 2);
    }

    public override void setProperty(int property)
    {
        this.property = property;
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("blocks/OrgBlock" + (property), typeof(Sprite));
    }

    public override BlockType returnType()
    {
        return BlockType.ORG;
    }
}
