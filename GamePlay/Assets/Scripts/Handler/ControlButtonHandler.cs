using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButtonHandler : MonoBehaviour
{
    public Vector3 direction;

    public void move()
    {
        Player player = Database.Player.GetComponent<Player>();

        if (player.GetCanMove())
        {
            player.SetDirection(direction);
            player.move(direction);
        }
    }
}
