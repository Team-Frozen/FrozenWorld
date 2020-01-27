using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    Vector3 vec_dir;
    float maxVelocity = 20f;
    public GameManager manager;
    float h = 0;
    float v = 0;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rigid.velocity == Vector3.zero)     //속도가 0일 때만 이동
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            if (h == 0 || v == 0)       //수직, 수평 동시에 눌린 경우 제외
            {
                vec_dir = new Vector3(h, 0, v);
                rigid.AddForce(vec_dir, ForceMode.Impulse);
                manager.moves++;
            }
        }
        else if (rigid.velocity.sqrMagnitude < maxVelocity)
            rigid.AddForce(vec_dir, ForceMode.Impulse);
    }

    public void SetVelocityZero()
    {
        rigid.velocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            SceneManager.LoadScene(manager.currentStage + 1);
        }
    }
}
