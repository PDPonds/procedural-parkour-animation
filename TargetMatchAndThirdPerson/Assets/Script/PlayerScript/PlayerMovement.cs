using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MainObserver
{
    [HideInInspector] public Vector3 moveDir;
    void Update()
    {
        MovementAndRotation();
        ActiveAllObserver(PlayerAction.Walk);
    }

    void MovementAndRotation()
    {
        Vector3 dir = new Vector3(PlayerManager.Instance.moveInput.x, 0, PlayerManager.Instance.moveInput.y);
        dir.Normalize();
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;

        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelative = dir.z * forward;
        Vector3 rightRelative = dir.x * right;
        moveDir = forwardRelative + rightRelative;

        if (dir.magnitude > 0 && PlayerManager.Instance.CanMove)
        {
            if (PlayerManager.Instance.isRun)
            {
                PlayerManager.Instance.currentSpeed = PlayerManager.Instance.runSpeed;
            }
            else
            {
                PlayerManager.Instance.currentSpeed = PlayerManager.Instance.walkSpeed;
            }

            transform.Translate(moveDir * PlayerManager.Instance.currentSpeed * Time.deltaTime, Space.World);

        }

        if (!PlayerManager.Instance.isAim && PlayerManager.Instance.CanMove)
        {
            if (moveDir != Vector3.zero && !PlayerManager.Instance.isAim)
            {
                moveDir.Normalize();
                moveDir.y = 0;
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, PlayerManager.Instance.rotationDamp * Time.deltaTime);
                transform.rotation = playerRot;
            }
        }

    }

}
