using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void move(RaycastHit hitInfo)
    {
       this.transform.position = new Vector3(Mathf.Round(hitInfo.point.x), 1, Mathf.Round(hitInfo.point.z));
    }
    

}
