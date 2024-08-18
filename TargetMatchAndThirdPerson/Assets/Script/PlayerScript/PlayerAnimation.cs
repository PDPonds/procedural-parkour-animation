using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour, IObserver
{
    public MainObserver PlayerMovementObserver;
    public MainObserver PlayerShootObserver;
    public MainObserver PlayerClimbUpObserver;
    public MainObserver PlayerJumpObserver;
    public MainObserver PlayerAttackObserver;
    #region - Movement Variable -
    float speed;
    float aim;
    float moveX;
    float moveZ;
    #endregion


    private void OnEnable()
    {
        PlayerMovementObserver.AddObserver(this);
        PlayerShootObserver.AddObserver(this);
        PlayerClimbUpObserver.AddObserver(this);
        PlayerJumpObserver.AddObserver(this);
        PlayerAttackObserver.AddObserver(this);
    }

    private void OnDisable()
    {
        PlayerMovementObserver.RemoveObserver(this);
        PlayerShootObserver.RemoveObserver(this);
        PlayerClimbUpObserver.RemoveObserver(this);
        PlayerJumpObserver.RemoveObserver(this);
        PlayerAttackObserver.RemoveObserver(this);
    }

    public void FuncToDo(PlayerAction action)
    {
        switch (action)
        {
            case PlayerAction.Walk:
                if (PlayerManager.Instance.CanMove)
                {
                    if (PlayerManager.Instance.isAim)
                    {
                        aim = Mathf.Lerp(aim, 1, 10 * Time.deltaTime);

                        Vector2 moveDir = PlayerManager.Instance.moveInput.normalized;

                        moveX = Mathf.Lerp(moveX, moveDir.x, 10 * Time.deltaTime);
                        moveZ = Mathf.Lerp(moveZ, moveDir.y, 10 * Time.deltaTime);

                    }
                    else
                    {
                        aim = Mathf.Lerp(aim, 0, 5 * Time.deltaTime);

                        if (PlayerManager.Instance.moveInput.magnitude == 0)
                        {
                            speed = Mathf.Lerp(speed, 0, 10 * Time.deltaTime);
                        }
                        else
                        {
                            if (PlayerManager.Instance.currentSpeed == PlayerManager.Instance.walkSpeed)
                            {
                                speed = Mathf.Lerp(speed, 0.5f, 10 * Time.deltaTime);
                            }
                            else if (PlayerManager.Instance.currentSpeed == PlayerManager.Instance.runSpeed)
                            {
                                speed = Mathf.Lerp(speed, 1f, 10 * Time.deltaTime);
                            }
                        }
                    }

                }
                else
                {
                    aim = Mathf.Lerp(aim, 0, 10 * Time.deltaTime);
                    speed = Mathf.Lerp(speed, 0, 10 * Time.deltaTime);
                    moveX = Mathf.Lerp(moveX, 0, 10 * Time.deltaTime);
                    moveZ = Mathf.Lerp(moveZ, 0, 10 * Time.deltaTime);
                }

                PlayerManager.Instance.animator.SetFloat("Aim", aim);
                PlayerManager.Instance.animator.SetFloat("Speed", speed);
                PlayerManager.Instance.animator.SetFloat("MoveX", moveX);
                PlayerManager.Instance.animator.SetFloat("MoveZ", moveZ);

                break;
            case PlayerAction.Aim:

                if (!PlayerManager.Instance.isAim)
                {
                    PlayerManager.Instance.ArrowTranformInHand.gameObject.SetActive(false);
                }

                break;
            case PlayerAction.Shoot:

                PlayerManager.Instance.animator.ResetTrigger("Shoot");
                PlayerManager.Instance.animator.SetTrigger("Shoot");

                break;
            case PlayerAction.ClimbUp:

                StartCoroutine(ClimbUpAction("ClimbUp"));

                break;
            case PlayerAction.ClimbUpHeight:

                StartCoroutine(ClimbUpAction("ClimbHoldUp"));

                break;
            case PlayerAction.Jump:

                StartCoroutine(Jump());

                break;
            case PlayerAction.JumpForward:

                StartCoroutine(ClimbUpAction("JumpForward"));

                break;
            case PlayerAction.Attack:

                PlayerManager.Instance.animator.runtimeAnimatorController = PlayerManager.Instance.combo[PlayerManager.Instance.combatCount].animOV;
                PlayerManager.Instance.animator.CrossFade("Attack", 0.1f);

                break;
            default:

                break;
        }
    }


    public IEnumerator Jump()
    {
        PlayerManager.Instance.isOnAir = true;
        PlayerManager.Instance.rb.isKinematic = false;
        PlayerManager.Instance.animator.applyRootMotion = true;
        PlayerManager.Instance.animator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.8f);
        PlayerManager.Instance.animator.ResetTrigger("Jump");
        PlayerManager.Instance.rb.isKinematic = false;
        PlayerManager.Instance.animator.applyRootMotion = false;
        PlayerManager.Instance.isOnAir = false;

    }

    public IEnumerator ClimbUpAction(string animName)
    {
        PlayerManager.Instance.isClimb = true;
        PlayerManager.Instance.rb.isKinematic = true;
        PlayerManager.Instance.animator.CrossFade(animName, .02f);
        yield return null;
        PlayerManager.Instance.CanMove = false;
        PlayerManager.Instance.animator.applyRootMotion = true;

        yield return new WaitForSeconds(PlayerManager.Instance.animator.GetNextAnimatorStateInfo(0).length + 0.1f);

        PlayerManager.Instance.animator.applyRootMotion = false;
        PlayerManager.Instance.CanMove = true;
        PlayerManager.Instance.isClimb = false;
        PlayerManager.Instance.rb.isKinematic = false;
    }

    public void ActiveArrow()
    {
        PlayerManager.Instance.ArrowTranformInHand.gameObject.SetActive(true);
    }

    public void DeActiveArrow()
    {
        PlayerManager.Instance.ArrowTranformInHand.gameObject.SetActive(false);
    }

}
