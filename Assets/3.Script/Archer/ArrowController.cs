using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private ArrowSpawner arrowSpawner;
    private Player player;
    private Vector3 direction;
    private SpriteRenderer spriteRenderer;
    private float currentPos;

    [SerializeField]
    float moveSpeed;

    private void OnEnable()
    {
        

    }

    private void Start()
    {
        arrowSpawner = GameObject.Find("ArrowSpawner").GetComponent<ArrowSpawner>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (player.transform.rotation.y == 0)
        {
            direction = Vector3.left;
            spriteRenderer.flipX = true;
        }
        else
        {
            direction = Vector3.right;
        }

        currentPos = player.transform.position.x;
    }

    private void Update()
    {
        if (transform.position.x > currentPos + 10f || transform.position.x < currentPos - 10f)
        {
            arrowSpawner.InsertQueue(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            arrowSpawner.InsertQueue(gameObject);
        }

    }
}
