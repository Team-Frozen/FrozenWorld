using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Element
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
        return BlockType.EXIT;
    }
    public override bool inValidArea(Stage stage)
    {
        int size = stage.getStageSize();
        if (transform.position.x == size * 0.5f + 0.5f &&
            transform.position.z <  size * 0.5f &&
            transform.position.z > -size * 0.5f)
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
        this.gameObject.transform.position = new Vector3(return_x, this.gameObject.transform.position.y, return_z);
        base.setVisible();
    }
}
