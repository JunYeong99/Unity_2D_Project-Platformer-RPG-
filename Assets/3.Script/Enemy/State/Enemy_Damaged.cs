using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Damaged : Enemy_State
{
    Enemy_StatePattern enemy_StatePattern;
    SpriteRenderer[] spriteRenderer;

    Coroutine coroutine;

    private void Awake()
    {
        enemy_StatePattern = GetComponent<Enemy_StatePattern>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }

    public override void OnStateEnter()
    {
        enemy_StatePattern.anim.SetTrigger("doDamaged");
        StartCoroutine(Damaged());
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
        
    }

    IEnumerator Damaged()
    {
        enemy_StatePattern.isDamaged = true;
        gameObject.layer = 12;

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 0.7f);
        }

        yield return new WaitForSeconds(0.5f);

        enemy_StatePattern.isTarget = true;

        OffDamaged();
    }

    void OffDamaged()
    {
        enemy_StatePattern.isDamaged = false;
        gameObject.layer = 9;

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 1);
        }

        OnStateExit();
    }
}
