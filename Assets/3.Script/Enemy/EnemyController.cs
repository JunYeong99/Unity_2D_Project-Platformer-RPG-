using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Enemy_State preState;
    Enemy_State tempState;
    Enemy_State currentState;
      
    public void SetState(Enemy_State enemy_state)
    {
        tempState = currentState;
        currentState = enemy_state;
        preState = tempState;
    }

    public void Action()
    {
        if(preState != null)
        {
            preState.OnStateExit();
        }

        currentState.OnStateEnter();
    }
}
