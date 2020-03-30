using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlpBlock : Block
{
    public override void changeProperty()
    {
        property++;
        property = property % 4;
        this.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
    }

    public override void setProperty(int property)
    {
        this.property = property;
        this.transform.Rotate(0.0f, 90.0f * property, 0.0f, Space.Self);
    }

    public override BlockType returnType()
    {
        return BlockType.SLP;
    }
}
