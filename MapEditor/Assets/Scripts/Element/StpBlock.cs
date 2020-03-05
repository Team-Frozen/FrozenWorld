using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StpBlock : Block
{
    public override void changeProperty()
    {
        property++;
    }

    public override void setProperty(int property)
    {
        this.property = property;
        this.transform.Rotate(0.0f, 90.0f * property, 0.0f, Space.Self);
    }

    public override BlockType returnType()
    {
        return BlockType.STP;
    }
}
