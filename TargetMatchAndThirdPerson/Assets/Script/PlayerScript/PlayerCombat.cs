using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MainObserver
{
    public bool canClick;
    public bool comboTime;

    public float currentCanClickDelay;
    public float currentComboTimeDelay;

    private void Update()
    {
        PlayerManager.Instance.combatCount = Mathf.Clamp(PlayerManager.Instance.combatCount, 0, PlayerManager.Instance.combo.Count);

        if (!canClick)
        {
            currentCanClickDelay -= Time.deltaTime;
            if (currentCanClickDelay <= 0)
            {
                canClick = true;
            }
        }

        if (!comboTime)
        {
            currentComboTimeDelay -= Time.deltaTime;
            if (currentComboTimeDelay <= 0)
            {
                comboTime = true;
                PlayerManager.Instance.CanMove = true;
                PlayerManager.Instance.combatCount = 0;
            }
        }

    }

    public void Attack()
    {
        if (PlayerManager.Instance.combatCount < PlayerManager.Instance.combo.Count)
        {
            if (canClick)
            {
                PlayerManager.Instance.CanMove = false;
                ActiveAllObserver(PlayerAction.Attack);
                currentCanClickDelay = PlayerManager.Instance.nextFireTime;
                currentComboTimeDelay = PlayerManager.Instance.comboTime;
                canClick = false;
                comboTime = false;
                PlayerManager.Instance.combatCount++;
            }
        }

    }


}
