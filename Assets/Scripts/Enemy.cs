/*
 * Office Zombie - Enemy.cs
 * 적 속성 및 기능 스크립트
 * 부모 클래스 : Entity 
 * 20170804 이학성
 * 배회하기, 유저 쫓아가기
 * 20170807 : 총알충돌 판단 구현
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//적의 상태 (배회, 발견, 공격)
public enum ENEMYSTATE { WANDER = 0, DISCOVER ,ATTACK, MOVE }

public class Enemy : Entity
{
    Transform target;
    NavMeshAgent nmAgent;
    Vector3 WanderTargetPos;
    public ENEMYSTATE EnemyState;
    float MoveTime;
    float Howling;

    public void Init(Vector3 _pos)
    {
        transform.position = _pos;
        //이동속도
        moveSpeed = 2.0f;
        //AI를 위한 NavMeshAgent
		nmAgent = GetComponent<NavMeshAgent>();
		nmAgent.enabled = true;
        //유저 위치 확인
		target = GameObject.Find("Character").transform;
        //적 상태, 초기값 배회하기
        EnemyState = ENEMYSTATE.WANDER;
        //배회하기
        //UpdateWanderPos();
        //일정 시간 움직이지 않으면 이동
        MoveTime = 0;
    }
    void Update()
    {
        StateUpdate();
        MoveTime += 1.0f * Time.deltaTime;
    }
    //상태 확인후 행동
    void StateUpdate()
    {
        switch (EnemyState)
        {
            case ENEMYSTATE.WANDER:
                //목표위치 도달 확인
                /*if ((nmAgent.destination - transform.position).sqrMagnitude < 0.001f)
                {
                    Debug.Log("WANDER");
                    transform.position = nmAgent.destination;
                    UpdateWanderPos();
                }*/
                if (MoveTime > 1.0f)
                {
                    //장애물 처리
                    UpdateWanderPos();
                    MoveTime = 0;
                }
                break;
            case ENEMYSTATE.DISCOVER:
                    UpdateDiscover();
                    Debug.Log("DISCOVER");
                break;
        }
    }
    //발견 후 유저판단
    void UpdateDiscover()
    {
        //자신의 앞 부분
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        //벽 확인을 위한 raycast
        Vector3 center = GetComponent<Collider>().bounds.center;
        if (Physics.Raycast(center, forward, out hit, Mathf.Infinity, 1<<LayerMask.NameToLayer("User")))
        {
            Debug.Log(hit.transform.name);
            //raycast 한 것이 유저인가?
            if (hit.collider.tag.Equals("USER"))
            {
                Debug.Log("USER RAY CAST");
                //유저의 위치
                Vector3 goalDir = target.position - transform.position;
                //y값은 고정하기 위해서
                goalDir.y = 0.0f;
                //유저의 위치 방향으로
                moveDirection = goalDir.normalized;
                //유저방향
                transform.rotation = Quaternion.LookRotation(goalDir);
                //유저로 이동
                nmAgent.SetDestination(target.position);
            }
        }
    }
    //배회하기 위치 조절
    void UpdateWanderPos()
    {
        //현재 위치로부터 목표지점까지의 거리
        float wanderRadius = 5.0f;
        //이동방향
        int wanderJitter = 0;
        //이동방향 범위
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        //현재 적 캐릭터가 밟고 있는 땅의 위치와 크기(구역정하기)
        Vector3 rangePos = new Vector3(18.0f, 0.0f, 18.0f);// Vector3.zero;
        Vector3 rangeScale = new Vector3(36.0f, 0.0f, 36.0f);// Vector3.one * 30.0f;

        //자신을 중심으로 3.0f거리 만큼 떨어진 위치를 목표위치로 설정(이동방향 : 0~360도)
        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        WanderTargetPos = transform.position + SetAngle(wanderRadius, wanderJitter);
        //생성된 목표위치가 자신의 구역을 벗어나지 않게 조절
        WanderTargetPos.x = Mathf.Clamp(WanderTargetPos.x, rangePos.x - rangeScale.x * 0.5f + 1.0f, rangePos.x + rangeScale.x * 0.5f - 1.0f);
         WanderTargetPos.y = 0f;
         WanderTargetPos.z = Mathf.Clamp(WanderTargetPos.z, rangePos.z - rangeScale.z * 0.5f + 1.0f, rangePos.z + rangeScale.z * 0.5f - 1.0f);
         
        //목표위치 설정, 목표위치를 바라보도록 함
        nmAgent.SetDestination(WanderTargetPos);
        transform.rotation = Quaternion.LookRotation(WanderTargetPos - transform.position);
    }
    //배회 반지름 계산
    Vector3 SetAngle(float _radius, int _angle)
    {
        Vector3 pos = Vector3.zero;

        pos.x = Mathf.Cos(_angle) * _radius;
        pos.z = Mathf.Sin(_angle) * _radius;

        return pos;
    }
}
