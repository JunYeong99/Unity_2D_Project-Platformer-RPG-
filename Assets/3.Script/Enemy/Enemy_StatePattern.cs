using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StatePattern : MonoBehaviour
{
    private float MaxHP = 100f;
    private float currentHP;

    public float MAXHP => MaxHP;
    public float CurrentHP => currentHP;

    [SerializeField]
    EnemyController enemyController;

    [SerializeField]
    Enemy_State enemy_Patrol;
    [SerializeField]
    Enemy_State enemy_Chaser;
    [SerializeField]
    Enemy_State enemy_Attack;
    [SerializeField]
    Enemy_State enemy_Damaged;
    [SerializeField]
    Enemy_State enemy_Die;

    public Animator anim;

    public Rigidbody2D rigid;

    public int nextMove;
    public float speed;
    
    public bool isAttack;
    public bool isDamaged;
    public bool isTarget;
    public bool isDie;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        enemy_Patrol = GetComponent<Enemy_Patrol>();
        enemy_Chaser = GetComponent<Enemy_Chaser>();
        enemy_Attack = GetComponent<Enemy_Attack>();
        enemy_Damaged = GetComponent<Enemy_Damaged>();
        enemy_Die = GetComponent<Enemy_Die>();

        currentHP = MaxHP;

        isAttack = false;
        isDamaged = false;
        isTarget = false;
        isDie = false;
    }

    private void Start()
    {
        EnemyPatrol();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHP <= 0 && !isDie)
        {
            EnemyDie();
        }
        else if (currentHP > 0 && !isDie && !isDamaged)
        {
            if (isTarget)
            {
                EnemyChase();
            }
            else
            {
                EnemyPatrol();
            }
        }

        

        Vector2 Vec = new Vector2(rigid.position.x, rigid.position.y + 1f);

        if (nextMove == 1)
        {
            RaycastHit2D Hit = Physics2D.Raycast(Vec, Vector2.right, 3f, LayerMask.GetMask("Player"));
            Debug.DrawRay(Vec, Vector2.right * 3f, new Color(0, 1, 0));
            if (Hit.collider != null)
            {
                isTarget = true;
            }
        }
        else
        {
            RaycastHit2D Hit = Physics2D.Raycast(Vec, Vector2.left, 3f, LayerMask.GetMask("Player"));
            Debug.DrawRay(Vec, Vector2.left * 3f, new Color(0, 1, 0));
            if (Hit)
            {
                isTarget = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            Debug.Log(collision.gameObject.tag);

            currentHP -= 30f;

            if (currentHP > 0)
                EnemyDamged();

        }

        if (collision.gameObject.tag == "Skill")
        {
            Debug.Log(collision.gameObject.tag);

            currentHP -= 80f;

            if (currentHP > 0)
                EnemyDamged();
        }
    }

    void EnemyChase()
    {
        enemyController.SetState(enemy_Chaser);
        enemyController.Action();
    }

    void EnemyPatrol()
    {
        enemyController.SetState(enemy_Patrol);
        enemyController.Action();
    }

    void EnemyDamged()
    {
        enemyController.SetState(enemy_Damaged);
        enemyController.Action();
    }

    void EnemyDie()
    {
        enemyController.SetState(enemy_Die);
        enemyController.Action();
    }
}
