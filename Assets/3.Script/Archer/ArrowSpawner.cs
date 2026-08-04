using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrow_Prefabs;

    public int count = 15;

    private Queue<GameObject> arrow_Box = new Queue<GameObject>();

    private Vector2 PoolPosition = new Vector2(-100f, 0);

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject arrow = Instantiate(arrow_Prefabs, PoolPosition, Quaternion.identity, transform);
            arrow_Box.Enqueue(arrow);
            arrow.SetActive(false);
        }
    }

    public void InsertQueue(GameObject insert_arrow) // 사용한 객체를 Pool(큐)에 반납시키는 함수
    {
        arrow_Box.Enqueue(insert_arrow);
        insert_arrow.SetActive(false);
    }
    public GameObject GetQueue()
    {
        GameObject used_arrow = arrow_Box.Dequeue();
        used_arrow.SetActive(true);

        return used_arrow;
    }
}
