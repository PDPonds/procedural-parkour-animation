using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour, IObserver
{
    public MainObserver PlayerAimShootObserver;
    private void OnEnable()
    {
        PlayerAimShootObserver.AddObserver(this);
    }

    private void OnDisable()
    {
        PlayerAimShootObserver.RemoveObserver(this);
    }

    public void FuncToDo(PlayerAction action)
    {
        switch (action)
        {
            case PlayerAction.Aim:

                if (PlayerManager.Instance.isAim)
                {
                    PlayerManager.Instance.Crosshair.SetActive(true);
                }
                else
                {
                    PlayerManager.Instance.Crosshair.SetActive(false);
                }

                break;

            default:
                break;
        }
    }
}
