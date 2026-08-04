using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Patrol : Enemy_State
{
    Enemy_StatePattern enemy_StatePattern;

    SpriteRenderer[] spriteRenderer;

    Coroutine coroutine;

    private void Awake()
    {
        enemy_StatePattern = GetComponent<Enemy_StatePattern>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();

        Invoke("NextAction", 2);
    }

    public override void OnStateEnter()
    {
        enemy_StatePattern.anim.SetInteger("isWalk", enemy_StatePattern.nextMove);
        OnStateUpdate();
    }

    public override void OnStateExit()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public override void OnStateUpdate()
    {
        if(coroutine == null)
        {
            coroutine = StartCoroutine(Patrol());
        }
    }

    IEnumerator Patrol()
    {
        while(true)
        {
            //Move
            enemy_StatePattern.rigid.velocity = new Vector2(enemy_StatePattern.nextMove, enemy_StatePattern.rigid.velocity.y);

            Vector2 frontVec = new Vector2(enemy_StatePattern.rigid.position.x + enemy_StatePattern.nextMove * 0.2f, enemy_StatePattern.rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

            if (rayHit.collider == null)
            {
                NextAction();
            }
                
            yield return null;
        }
    }

    void NextAction()
    {
        //Set Next Active
        //Range(): 최소 ~ 최대 범위의 랜덤 수 생성(최대 제외)
        enemy_StatePattern.nextMove = Random.Range(-1, 2);

        //Flip Sprite
        if (enemy_StatePattern.nextMove == -1)
        {
            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
        else
        {
            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
        }

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("NextAction", nextThinkTime);
    }
}
