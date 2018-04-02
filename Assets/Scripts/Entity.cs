/*
 * Office Zombie - Entity.cs
 * 움직이는 오브젝트 공통 스크립트(상속용)
 * 20170804 이학성
 * 데이터
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    //점프 후 떨어지는 중력값
    protected readonly float gravity = -20.0f;
    protected float moveSpeed;
    protected Vector3 moveDirection;
}
