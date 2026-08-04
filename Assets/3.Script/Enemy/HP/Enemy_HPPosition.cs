using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_HPPosition : MonoBehaviour
{
    [SerializeField] private Vector3 distance = Vector3.up * 35f;

    [SerializeField]
    private GameObject Target;
    [SerializeField]
    private RectTransform UItransform;

    private void Awake()
    {
        UItransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(Target == null) 
        {
            Destroy(gameObject);
        }

        //WorldToScreenPoint: 카메라상에서 오브젝트의 포지션을
        //포인트로 잡아서 -> vector3로 반환해준다.
        Vector3 screenPosition =
            Camera.main.WorldToScreenPoint(Target.transform.position);

        UItransform.position = screenPosition + distance;
    }
}
