using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float MaxHP = 100f;
    private float currentHP;

    public float MAXHP => MaxHP;
    public float CurrentHP => currentHP;

    public int nextMove;

    public float speed;
    public float stopDistance;

    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    SpriteRenderer[] spriteRenderer;
    Animator anim;
    
    [SerializeField]
    Transform player;

    public bool isDamaged;
    public bool isDie;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        boxCollider = GameObject.FindWithTag("EnemyAttack").GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
       

        currentHP = MaxHP;

        boxCollider.enabled = false;
        isDamaged = false;
        isDie = false;
        //Invoke(): 주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수
        Invoke("Think", 1f);
    }

    private void Update()
    {
        if(isDie)
        {
            return;
        }

        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        if (isDamaged && !isDie) 
        {
            chaser();
        }
    }
    void FixedUpdate()
    {
        Vector2 Vec = new Vector2(rigid.position.x, rigid.position.y + 1f);

        if(!isDamaged && !isDie) 
        {
            if (nextMove == -1)
            {
                RaycastHit2D Hit = Physics2D.Raycast(Vec, Vector2.left, 3f, LayerMask.GetMask("Player"));
                Debug.DrawRay(Vec, Vector2.left * 3f, new Color(0, 1, 0));
                if (Hit.collider != null)
                {
                    chaser();
                    CancelInvoke();
                }
                else
                {
                    Patrol();
                }
            }
            else
            {
                RaycastHit2D Hit = Physics2D.Raycast(Vec, Vector2.right, 3f, LayerMask.GetMask("Player"));
                Debug.DrawRay(Vec, Vector2.right * 3f, new Color(0, 1, 0));
                if (Hit)
                {
                    chaser();
                    CancelInvoke();
                }
                else
                {
                    Patrol();
                }
            }
        }        
    }

    void Patrol()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (rayHit.collider == null)
            turn();
    }

    //재귀 함수: 자신을 스스로 호출하는 함수
    void Think()
    {
        //Set Next Active
        //Range(): 최소 ~ 최대 범위의 랜덤 수 생성(최대 제외)
        nextMove = Random.Range(-1, 2);

        //Sprite Animation
        anim.SetInteger("isWalk", nextMove);

        //Flip Sprite
        if (nextMove == -1)
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
        Invoke("Think", nextThinkTime);
    }

    void turn()
    {
        nextMove = nextMove * -1;

        if(nextMove == -1)
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
        
        CancelInvoke();

        Invoke("Think", 2);
    }

    void chaser()
    {
        CancelInvoke();
        turning();

        Vector3 currentPos = transform.position;

        float targetPos = player.position.x;
        float moveX = Mathf.MoveTowards(currentPos.x, targetPos, speed * Time.deltaTime);

        transform.position = new Vector3(moveX, currentPos.y, currentPos.z);

        if (Mathf.Abs(currentPos.x - targetPos) < 0.5f)
        {
            //EnemyAttack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {     
        if (collision.gameObject.tag == "Weapon")
        {
            currentHP -= 30f;
            isDamaged = true;
            OnDamaged();

            if(currentHP <= 0)
            {
                OnDie();
            }
        }

    }


    public void OnDamaged()
    {
        anim.SetTrigger("doDamaged");
        gameObject.layer = 12;

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 0.7f);
        }

        Invoke("IsDamaged", 0.2f);
        //Destroy
        Invoke("OffDamaged", 0.5f);
    }

    void IsDamaged()
    {

    }

    void OffDamaged()
    {
        gameObject.layer = 9;

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 1);
        }
    }

    void OnDie()
    {
        isDie = true;
        anim.SetTrigger("doDie");
        Invoke("ObjectFalse", 5f);
    }

    void ObjectFalse()
    {
        gameObject.SetActive(false);
    }

    IEnumerator Attack()
    {
        yield return null;

        boxCollider.enabled = true;
    }

    void turning()
    {
        if (player.position.x - transform.position.x > 0)
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
