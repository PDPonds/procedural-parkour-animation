using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParkoutState
{
    none, ClimbUp, ClimUpHeight, JumpForward
}

public class PlayerClimb : MainObserver
{
    [HideInInspector] public Vector3 climbUpPos;
    Vector3 climbPoint;


    private void Update()
    {
        #region Can Use but Raycast Check
        PlayerManager.Instance.forwardPos.localPosition = new Vector3(0, .1f, 0);
        PlayerManager.Instance.headPos.localPosition = new Vector3(0, PlayerManager.Instance.headHeight, 0);

        if (Physics.Raycast(PlayerManager.Instance.forwardPos.position, transform.forward, out RaycastHit hit, PlayerManager.Instance.checkLenght * 2, PlayerManager.Instance.obstacleMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.CompareTag("JumpForward"))
            {
                PlayerManager.Instance.topPos.position = hit.point + new Vector3(0, PlayerManager.Instance.maxHeight, 0);
                Debug.DrawRay(PlayerManager.Instance.forwardPos.position, transform.forward * PlayerManager.Instance.checkLenght * 2, Color.red);
                if (Physics.Raycast(PlayerManager.Instance.topPos.position, Vector3.down, out RaycastHit hitInfo, PlayerManager.Instance.maxHeight, PlayerManager.Instance.obstacleMask, QueryTriggerInteraction.Ignore))
                {
                    climbPoint = hitInfo.point + transform.forward * .1f;
                    if (Vector3.Distance(hit.point, transform.position) > 1f && !PlayerManager.Instance.isOnAir)
                    {
                        PlayerManager.Instance.parkoutState = ParkoutState.JumpForward;

                    }
                    else
                    {
                        PlayerManager.Instance.parkoutState = ParkoutState.none;
                    }
                }

            }
            else if (!hit.transform.CompareTag("JumpForward") && Vector3.Distance(hit.point, transform.position) <= 1)
            {
                Debug.DrawRay(PlayerManager.Instance.forwardPos.position, transform.forward * PlayerManager.Instance.checkLenght * 2, Color.blue);
                PlayerManager.Instance.topPos.position = hit.point + new Vector3(0, PlayerManager.Instance.maxHeight, 0);
                Debug.DrawRay(PlayerManager.Instance.topPos.position, Vector3.down * PlayerManager.Instance.maxHeight, Color.green);
                if (Physics.Raycast(PlayerManager.Instance.topPos.position, Vector3.down, out RaycastHit hitInfo, PlayerManager.Instance.maxHeight, PlayerManager.Instance.obstacleMask, QueryTriggerInteraction.Ignore))
                {
                    float yHeadToTop = PlayerManager.Instance.topPos.position.y - PlayerManager.Instance.headPos.position.y;

                    if (Physics.Raycast(PlayerManager.Instance.headPos.position, Vector3.up, yHeadToTop, PlayerManager.Instance.obstacleMask, QueryTriggerInteraction.Ignore))
                    {
                        PlayerManager.Instance.parkoutState = ParkoutState.none;
                    }

                    climbPoint = hitInfo.point + transform.forward * .1f;

                    if (!PlayerManager.Instance.isOnAir)
                    {
                        if (Vector3.Distance(climbPoint, hit.point) > .5f && Vector3.Distance(climbPoint, hit.point) < 1.5f)
                        {
                            PlayerManager.Instance.parkoutState = ParkoutState.ClimbUp;
                        }
                        else if (Vector3.Distance(climbPoint, hit.point) >= 1.5f)
                        {
                            PlayerManager.Instance.parkoutState = ParkoutState.ClimUpHeight;
                        }
                        else
                        {
                            PlayerManager.Instance.parkoutState = ParkoutState.none;
                        }
                    }
                    else
                    {
                        PlayerManager.Instance.parkoutState = ParkoutState.none;
                    }

                }
                else
                {
                    PlayerManager.Instance.parkoutState = ParkoutState.none;
                }
            }

        }
        else
        {
            PlayerManager.Instance.parkoutState = ParkoutState.none;
        }
        #endregion


        if (PlayerManager.Instance.animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbUp"))
        {
            MatchTargetWeightMask ClimbUpWeigh = new MatchTargetWeightMask(new Vector3(1, 1, 1), 0);
            PlayerManager.Instance.animator.MatchTarget(climbPoint, transform.rotation, AvatarTarget.RightFoot, ClimbUpWeigh, 0, .4f);
        }
        if (PlayerManager.Instance.animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbHoldUp"))
        {
            MatchTargetWeightMask ClimbUpWeigh = new MatchTargetWeightMask(new Vector3(1, 1, 1), 0);
            PlayerManager.Instance.animator.MatchTarget(climbPoint, transform.rotation, AvatarTarget.LeftFoot, ClimbUpWeigh, 0, .8f);
        }
        if (PlayerManager.Instance.animator.GetCurrentAnimatorStateInfo(0).IsName("JumpForward"))
        {
            MatchTargetWeightMask ClimbUpWeigh = new MatchTargetWeightMask(new Vector3(1, 1, 1), 0);
            PlayerManager.Instance.animator.MatchTarget(climbPoint, transform.rotation, AvatarTarget.LeftHand, ClimbUpWeigh, 0.2f, .5f);
        }
    }


    public void Climb(ParkoutState state)
    {
        switch (state)
        {
            case ParkoutState.none:
                break;
            case ParkoutState.ClimbUp:

                ActiveAllObserver(PlayerAction.ClimbUp);

                break;
            case ParkoutState.ClimUpHeight:

                ActiveAllObserver(PlayerAction.ClimbUpHeight);

                break;

            case ParkoutState.JumpForward:

                ActiveAllObserver(PlayerAction.JumpForward);

                break;

            default: break;
        }
    }

    public void JumpForward()
    {
        if (!PlayerManager.Instance.isOnAir)
        {
            ActiveAllObserver(PlayerAction.JumpForward);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(PlayerManager.Instance.topPos.position, .1f);
        Gizmos.DrawSphere(PlayerManager.Instance.forwardPos.position, .1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(climbPoint, .1f);
    }

}
