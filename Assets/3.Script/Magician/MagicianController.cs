using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private FireballSpawner fireballSpawner;
    private Player player;
    private Vector3 direction;
    private Transform transform;
    private float currentPos;

    [SerializeField]
    float moveSpeed;

    private void OnEnable()
    {
        

    }

    private void Start()
    {
        fireballSpawner = GameObject.Find("FireballSpawner").GetComponent<FireballSpawner>();
        transform = GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (player.transform.rotation.y == 0)
        {
            direction = Vector3.left;
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            direction = Vector3.right;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        currentPos = player.transform.position.x;
    }

    private void Update()
    {
        if (transform.position.x > currentPos + 10f || transform.position.x < currentPos - 10f)
        {
            fireballSpawner.InsertQueue(gameObject);
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
            fireballSpawner.InsertQueue(gameObject);
        }

    }
}
