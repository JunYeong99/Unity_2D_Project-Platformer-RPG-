using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SkillDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            
        }
    }
}
