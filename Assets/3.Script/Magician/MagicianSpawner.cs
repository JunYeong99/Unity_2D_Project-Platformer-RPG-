using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    public GameObject fireball_Prefabs;

    public int count = 15;

    private Queue<GameObject> fireball_Box = new Queue<GameObject>();

    private Vector2 PoolPosition = new Vector2(-100f, 0);

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject fireball = Instantiate(fireball_Prefabs, PoolPosition, Quaternion.identity, transform);
            fireball_Box.Enqueue(fireball);
            fireball.SetActive(false);
        }
    }

    public void InsertQueue(GameObject insert_fireball) // 사용한 객체를 Pool(큐)에 반납시키는 함수
    {
        fireball_Box.Enqueue(insert_fireball);
        insert_fireball.SetActive(false);
    }
    public GameObject GetQueue()
    {
        GameObject used_firball = fireball_Box.Dequeue();
        used_firball.SetActive(true);

        return used_firball;
    }
}
