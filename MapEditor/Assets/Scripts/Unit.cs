using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Element
{
    private float return_x, return_z;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override BlockType returnType()
    {
        return BlockType.UNIT;
    }

    public override bool inValidArea(Stage stage)
    {
        List<GameObject> elements = stage.getElements();
        int size = stage.getStageSize();
        if (inStage(size) && !onBlock(elements))  //Stage 안에 있고 블럭이 없는 곳
            return true;
        return false;
    }

    public override void setVisible()
    {
        return_x = this.gameObject.transform.position.x;
        return_z = this.gameObject.transform.position.z;
        base.setVisible();
    }

    public override void action()
    {
        this.gameObject.transform.position = new Vector3 (return_x, this.gameObject.transform.position.y, return_z);
        base.setVisible();
    }
}
