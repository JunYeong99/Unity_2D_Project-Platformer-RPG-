using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector2 center;
    [SerializeField]
    private Vector2 size;

    private float height;
    private float width;

    private void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    private void Update()
    {
        target = GameObject.FindWithTag("Player");
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.transform.position + new Vector3(0, 2f, 0);

        transform.position = Vector3.Lerp(targetPosition, targetPosition, Time.deltaTime * speed);

        //카메라 움직임 범위 제한
        float x = size.x * 0.5f - width;
        float clampX = Mathf.Clamp(transform.position.x, -x + center.x, x + center.x);

        float y = size.x * 0.5f - height;
        float clampY = Mathf.Clamp(transform.position.y, -y + center.y, y + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }
}
