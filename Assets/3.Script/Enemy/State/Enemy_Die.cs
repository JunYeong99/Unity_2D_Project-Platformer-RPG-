using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Die : Enemy_State
{
    Enemy_StatePattern enemy_StatePattern;
    Coroutine coroutine;
    Rigidbody2D rigid;

    private void Awake()
    {
        enemy_StatePattern = GetComponent<Enemy_StatePattern>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public override void OnStateEnter()
    {
        if(coroutine == null) 
        {
            Debug.Log("Á×À½");
            StartCoroutine(EnemyDie());
        }
        
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        
    }

    IEnumerator EnemyDie()
    {  
        gameObject.layer = 12;
        rigid.velocity = Vector3.zero;
        enemy_StatePattern.isDie = true;
        enemy_StatePattern.anim.SetTrigger("doDie");

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
