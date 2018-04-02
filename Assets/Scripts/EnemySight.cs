using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {
    Enemy e;

    void Awake()
    {
        e = GetComponentInParent<Enemy>();
    }
    //좀비 시야BOX 충돌상태
    void OnTriggerStay(Collider col)
    {
        //유저인가?
        if (col.tag.Equals("USER"))
        {
            e.EnemyState = ENEMYSTATE.DISCOVER;
        }
    }
    //충돌하는 것이 없으면 다시 배회하기
    void OnTriggerExit(Collider col)
    {
        e.EnemyState = ENEMYSTATE.WANDER;

    }
}
