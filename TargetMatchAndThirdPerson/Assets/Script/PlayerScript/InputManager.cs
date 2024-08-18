using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;
    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            playerInput.PlayerMove.Move.performed += i => PlayerManager.Instance.moveInput = i.ReadValue<Vector2>();
            playerInput.PlayerMove.View.performed += i => PlayerManager.Instance.viewInput = i.ReadValue<Vector2>();

            playerInput.Action.Aim.performed += i => ActiveAim();
            playerInput.Action.Aim.canceled += i => PlayerManager.Instance.isAim = false;

            playerInput.Action.Run.performed += i => ActiveRun();
            playerInput.Action.Run.canceled += i => PlayerManager.Instance.isRun = false;

            playerInput.Action.Shoot.performed += i => Shoot();

            playerInput.Action.Jump.performed += i => Jump();

        }

        playerInput.Enable();

    }


    void ActiveRun()
    {
        if (!PlayerManager.Instance.isAim)
        {
            PlayerManager.Instance.isRun = true;
        }
    }

    void ActiveAim()
    {
        if (!PlayerManager.Instance.isClimb)
        {
            PlayerManager.Instance.isAim = true;
            PlayerManager.Instance.ArrowTranformInHand.gameObject.SetActive(true);
        }

    }

    void Shoot()
    {
        if (PlayerManager.Instance.isAim)
        {
            PlayerManager.Instance.shoot.Shoot();

        }
        else
        {
            PlayerManager.Instance.combat.Attack();

        }
    }

    void Jump()
    {
        if (!PlayerManager.Instance.isClimb)
        {
            switch (PlayerManager.Instance.parkoutState)
            {
                case ParkoutState.none:

                    if (!PlayerManager.Instance.isAim)
                    {
                        PlayerManager.Instance.jump.Jump();
                    }

                    break;
                case ParkoutState.ClimbUp:

                    if (!PlayerManager.Instance.isAim && !PlayerManager.Instance.isClimb)
                    {
                        PlayerManager.Instance.climb.Climb(ParkoutState.ClimbUp);
                    }

                    break;
                case ParkoutState.ClimUpHeight:

                    if (!PlayerManager.Instance.isAim && !PlayerManager.Instance.isClimb)
                    {
                        PlayerManager.Instance.climb.Climb(ParkoutState.ClimUpHeight);
                    }
                    break;
                case ParkoutState.JumpForward:

                    if (!PlayerManager.Instance.isAim && !PlayerManager.Instance.isClimb)
                    {
                        PlayerManager.Instance.climb.JumpForward();
                    }

                    break;
                default: break;
            }

        }


    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

}
