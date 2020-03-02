using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StpBlock : Block
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void changeProperty()
    {
        property++;
        this.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
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
