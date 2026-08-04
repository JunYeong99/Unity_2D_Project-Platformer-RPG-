using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chaser : Enemy_State
{
    [SerializeField] Enemy_StatePattern enemy_StatePattern;
    [SerializeField] EnemyController enemyController;

    public BoxCollider2D boxCollider;

    [SerializeField]
    Transform player;

    SpriteRenderer[] spriteRenderer;

    Coroutine coroutine;

    private void Awake()
    {
        enemy_StatePattern = GetComponent<Enemy_StatePattern>();
        enemyController = GetComponent<EnemyController>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        
    }

    public override void OnStateEnter()
    {
        enemy_StatePattern.anim.SetInteger("isWalk", (int)enemy_StatePattern.speed);
        enemy_StatePattern.isTarget = true;

        if(coroutine == null)
        {
            OnStateUpdate();
        }
        
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
        if (coroutine == null)
        {
            coroutine = StartCoroutine("Chaser");
        }
    }

    IEnumerator Chaser()
    {
        while(true)
        {
            Trun();

            Vector3 currentPos = transform.position;

            float targetPos = player.position.x;
            float moveX = Mathf.MoveTowards(currentPos.x, targetPos, enemy_StatePattern.speed * Time.deltaTime);

            transform.position = new Vector3(moveX, currentPos.y, currentPos.z);

            if (Mathf.Abs(currentPos.x - targetPos) < 1.2f && !enemy_StatePattern.isAttack)
            {
                transform.position = currentPos;
                StartCoroutine(Attack());

                yield return new WaitForSeconds(1.2f);
            }

            yield return null;
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

    void Trun()
    {
        if (player.position.x - transform.position.x < 0)
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
    }

}
