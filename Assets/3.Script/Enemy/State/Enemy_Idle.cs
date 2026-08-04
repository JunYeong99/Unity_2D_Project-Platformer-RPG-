using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Idle : Enemy_State
{
    Enemy_StatePattern enemy_StatePattern;

    public override void OnStateEnter()
    {
        enemy_StatePattern.anim.SetInteger("isWalk", 0);
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        
    }
}
