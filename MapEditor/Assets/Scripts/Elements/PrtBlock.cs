using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrtBlock : Block
{
    PrtBlock linkedPrt;

    public override void changeProperty()
    {

    }

    public override BlockType returnType()
    {
        return BlockType.PRT;
    }

    public void setLinkedPrt(PrtBlock linked)
    {
        linkedPrt = linked;
    }

    public PrtBlock getLinkedPrt()
    {
        return linkedPrt;
    }
    public void setLinkedPortal(PrtBlock linked)
    {
        linkedPrt = linked;
    }

}
