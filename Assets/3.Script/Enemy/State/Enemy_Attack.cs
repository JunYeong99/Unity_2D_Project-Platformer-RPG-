using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : Enemy_State
{
    Enemy_StatePattern enemy_StatePattern;

    public BoxCollider2D boxCollider;

    Coroutine coroutine;

    EnemyController enemyController;

    Enemy_State enemy_Chaser;

    private void Awake()
    {
        enemy_StatePattern = GetComponent<Enemy_StatePattern>();
        enemyController = GetComponent<EnemyController>();
        enemy_Chaser = GetComponent<Enemy_Chaser>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void OnStateEnter()
    {
        OnStateUpdate();
    }

    public override void OnStateExit()
    {
        StopCoroutine(coroutine);
        coroutine = null;
    }

    public override void OnStateUpdate()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine("Attack");      
        }
    }

    IEnumerator Attack()
    {
        enemy_StatePattern.anim.SetTrigger("doAttack");
        enemy_StatePattern.isAttack = true;
        boxCollider.enabled = true;

        yield return new WaitForSeconds(0.8f);

        enemy_StatePattern.isAttack = false;
        boxCollider.enabled = false;
        OnStateExit();
    }
}
