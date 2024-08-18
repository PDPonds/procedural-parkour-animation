using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MainObserver
{

    public void Jump()
    {
        if (!PlayerManager.Instance.isOnAir)
        {
            ActiveAllObserver(PlayerAction.Jump);
        }
    }


}
