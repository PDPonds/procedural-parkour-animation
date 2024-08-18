using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MainObserver
{
    [HideInInspector] public float currentDelay;
    bool canShoot;
    private void Update()
    {
        ActiveAllObserver(PlayerAction.Aim);

        if (!canShoot)
        {
            currentDelay -= Time.deltaTime;
            if (currentDelay <= 0)
            {
                canShoot = true;
            }
        }
    }

    public void Shoot()
    {
        if (PlayerManager.Instance.isAim && canShoot)
        {
            currentDelay = PlayerManager.Instance.shootDelay;
            canShoot = false;
            RaycastHit hit;
            if (Physics.Raycast(GameManager.Instance.cameraTransform.position, GameManager.Instance.cameraTransform.forward, out hit, Mathf.Infinity))
            {
                ActiveAllObserver(PlayerAction.Shoot);
                GameObject arrow = Instantiate(PlayerManager.Instance.ArrowPrefab, PlayerManager.Instance.ArrowSpawnPoint.position, Quaternion.identity);
                ArrowController arrowCon = arrow.GetComponent<ArrowController>();
                if (arrowCon != null)
                {
                    arrowCon.movePoint = hit.point;
                }
            }
        }
    }

}
