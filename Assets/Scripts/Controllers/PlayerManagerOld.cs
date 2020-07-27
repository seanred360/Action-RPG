using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerOld : MonoBehaviour
{
   

    public GameObject player;

    public void KillPlayer()
    {
        // 0 is false 1 is true
        InputManager.instance.TogglePlayerInput(0);
    }
}
