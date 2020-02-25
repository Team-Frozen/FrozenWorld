using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Element
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //각 Block에 대한 스크립트 만들고 넣어야함
    public override BlockType returnType()
    {
        return BlockType.ORG;
    }

    public override bool inValidArea(Stage stage)
    {
        List<GameObject> elements = stage.getElements();
        int size = stage.getStageSize();
        if (inStage(size) && !onBlock(elements))  //Stage 안에 있고 블럭이 없는 곳
            return true;
        return false;
    }

    public override void action()
    {
        Test.Stage.GetComponent<Stage>().getElements().Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
