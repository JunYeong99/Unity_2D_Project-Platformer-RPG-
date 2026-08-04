using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float MaxHP = 100f;
    private float currentHP;

    public float MAXHP => MaxHP;
    public float CurrentHP => currentHP;

    public float maxSpeed;
    public float jumpPower;
    public float dashPower = 20f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f; 

    Rigidbody2D rigid;
    Transform transform;
    Animator anim;
    SpriteRenderer[] spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    Camera camera;

    ArrowSpawner arrowSpawner;
    FireballSpawner fireballSpawner;

    [SerializeField]
    GameObject arrowStartPoint;
    [SerializeField]
    GameObject fireballStartPoint;

    [SerializeField]
    GameObject wariiorSkill;

    [SerializeField]
    GameObject archerSkill_Attack;


    [SerializeField]
    GameObject magicianSkill_charging;
    [SerializeField]
    GameObject magicianSkill_Explosion;

    [SerializeField]
    SkillControl skill_CoolDown;

    bool doDash = true;
    bool isDash;
    bool isGround;
    bool isDamaged;
    bool isInPortal;

    [SerializeField]
    private Stage_Data stagedata;

    private void Awake()
    {
        currentHP = MaxHP;

        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        skill_CoolDown = GameObject.FindWithTag("Skill").GetComponent<SkillControl>();

        if (GameObject.Find("Archer(Clone)"))
        {
            arrowSpawner = GameObject.Find("ArrowSpawner").GetComponent<ArrowSpawner>();
        }

        else if (GameObject.Find("Magician(Clone)"))
        {
            fireballSpawner = GameObject.Find("FireballSpawner").GetComponent<FireballSpawner>();
        }

        isGround = false;
        isDamaged = false;
        isInPortal = false;
    }

    private void Update()
    {
        if (isInPortal && Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("Stage1");
        }

        if (!isDamaged)
        {
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGround && !isDamaged)
            {
                anim.SetTrigger("doJump");
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                isGround = false;
            }

            if(Input.GetKeyDown(KeyCode.LeftShift) && doDash)
            {
                StartCoroutine(Dash());
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }

            if(Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("doAttack");

                Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);

                if(transform.position.x > mousePosition.x)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    
                }
                if (GameObject.Find("Archer(Clone)"))
                    arrowSpawner.GetQueue().transform.position = arrowStartPoint.transform.position;
                else if (GameObject.Find("Magician(Clone)"))
                    fireballSpawner.GetQueue().transform.position = fireballStartPoint.transform.position;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (GameObject.Find("Warrior(Clone)") && skill_CoolDown.isHideSkills[0] == false)
                {
                    skill_CoolDown.isHideSkills[0] = true;
                    SkillAttack();
                    StartCoroutine("WariiorSkill");
                }

                if (GameObject.Find("Archer(Clone)") && skill_CoolDown.isHideSkills[1] == false)
                {
                    skill_CoolDown.isHideSkills[1] = true;
                    SkillAttack();
                    StartCoroutine("ArcherSkill");
                }

                if (GameObject.Find("Magician(Clone)") && skill_CoolDown.isHideSkills[2] == false)
                {
                    skill_CoolDown.isHideSkills[2] = true;
                    SkillAttack();
                    StartCoroutine("MagicianSkill");
                }
            }
        }
        
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalk", false);
        else
            anim.SetBool("isWalk", true);

    }

    private void FixedUpdate()
    {
        if(!isDamaged)
        {
            float MoveX = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * MoveX, ForceMode2D.Impulse);

            if (rigid.velocity.x > maxSpeed)
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < maxSpeed * (-1))
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }  
    }

    private void LateUpdate()
    {
        //플레이어가 화면 바깥으로 나가지 못하도록 설정
        transform.position =
            new Vector3
            (
                Mathf.Clamp(transform.position.x, stagedata.LimitMin.x, stagedata.LimitMax.x),
                Mathf.Clamp(transform.position.y, stagedata.LimitMin.y, stagedata.LimitMax.y),
                0
            );       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            isInPortal = true;
        }

        if (collision.gameObject.CompareTag("EnemyAttack"))
        {
            Debug.Log("ㅎㅇ");
            //OnDie();
            OnDamaged(collision.transform.position);
            currentHP -= 25f;

            if (currentHP <= 0)
            {
                OnDie();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            isInPortal = false;
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        isDamaged = true;

        //Change Layer (Immortal Active)
        gameObject.layer = 11;

        //피격 당할 시    
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 0.4f);
        }

        //작용 반작용
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 0.5f) * 7, ForceMode2D.Impulse);

        //애니메이션
        anim.SetTrigger("doDamaged");

        Invoke("IsDamaged", 0.5f);
        Invoke("OffDamaged", 1);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 1);
        }     
    }

    void IsDamaged()
    {
        isDamaged = false;
    }

    public void OnDie()
    {
        anim.SetTrigger("doDie");
    }

    void SkillAttack()
    {
        anim.SetTrigger("doSkillAttack");
    }

    IEnumerator Dash()
    {
        doDash = false;
        isDash = true;
        float originalGravity = rigid.gravityScale;
        float originalDrag = rigid.drag;
        rigid.gravityScale = 0f;
        rigid.drag = 0f;
        rigid.velocity = Vector2.zero;

        float speed = maxSpeed;

        maxSpeed = dashPower;

        yield return new WaitForSeconds(dashingTime);

        rigid.gravityScale = originalGravity;
        rigid.drag = originalDrag;
        isDash = false;

        maxSpeed = speed;

        yield return new WaitForSeconds(dashingCooldown);

        doDash = true;
    }

    IEnumerator WariiorSkill()
    {
        gameObject.layer = 11;
        wariiorSkill.SetActive(true);
        yield return new WaitForSeconds(10f);
        gameObject.layer = 10;
        wariiorSkill.SetActive(false);
    }

    IEnumerator ArcherSkill()
    {
        yield return new WaitForSeconds(0.5f);
        archerSkill_Attack.SetActive(true);
        yield return new WaitForSeconds(1f);
        archerSkill_Attack.SetActive(false);
    }

    IEnumerator MagicianSkill()
    {
        magicianSkill_charging.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        magicianSkill_charging.SetActive(false);
        magicianSkill_Explosion.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        magicianSkill_Explosion.SetActive(false);
    }


}
