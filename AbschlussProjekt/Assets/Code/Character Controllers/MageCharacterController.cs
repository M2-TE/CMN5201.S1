using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCharacterController : BaseCharacterController
{
    protected override void CheckRequestedActions()
    {
        base.CheckRequestedActions();
        TryMageBuff();
    }

    private void TryMageBuff()
    {
        if (Input.GetAxis("Fire2") > 0)
        {
            commandDelayCounter = attackDelay;
            ownAnimator.SetFloat("Movespeed", 0f);
            ownAnimator.SetTrigger("Buff");
        }
    }
}

