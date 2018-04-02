/*
 * Office Zombie - CameraManager.cs
 * 카메라 정면 조정 스크립트
 * 20170807 이학성
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] Transform target;
    Vector3 oriPos;

    void Awake()
    {
        oriPos = transform.position;
    }

    void Update()
    {
        transform.position = oriPos + target.position;

    }
}
