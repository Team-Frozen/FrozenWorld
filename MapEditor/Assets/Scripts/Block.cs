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
