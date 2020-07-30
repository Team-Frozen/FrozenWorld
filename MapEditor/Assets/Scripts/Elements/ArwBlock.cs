using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArwBlock : Block
{
    public override void changeProperty()
    {
        property++;
        property = property % 4;
        this.transform.GetChild(0).Rotate(0.0f, 0.0f, -90.0f, Space.Self);
    }

    public override void setProperty(int property)
    {
        this.property = property;
        this.transform.GetChild(0).Rotate(0.0f, 0.0f, -90.0f * (property + 1), Space.Self);
    }

    public override BlockType returnType()
    {
        return BlockType.ARW;
    }
}
