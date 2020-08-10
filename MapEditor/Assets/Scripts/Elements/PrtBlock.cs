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

    public void SetColor(int var)
    {
        switch (var)
        {
            case 0:
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                break;
            case 1:
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 0);
                break;
            case 2:
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 255, 100);
                break;
            default:
                break;
        }
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
