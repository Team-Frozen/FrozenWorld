using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButtonHandler : MonoBehaviour
{
    public Vector3 direction;

    public void move()
    {
        if (Player.canMove)
        {
            Database.Player.GetComponent<Player>().SetDirection(direction);
            Database.Player.GetComponent<Player>().move(direction);
        }
    }
}
