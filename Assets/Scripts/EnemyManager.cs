/*
 * Office Zombie - EnemyManager.cs
 * 적 생성 스크립트(Temp 사용미정)
 * 20170804 이학성
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //적 배열
    [SerializeField] GameObject[] Enemys;
    [SerializeField] GameObject Key;
    
    private int MaxZombie = 3;
    private int ZombieCount = 0;
    private float PosX;
    private float PosZ;
    private bool IsKey=false;

    void Awake()
    {
        InvokeRepeating("CreateEnemy", 0.0f, 3.0f );
    }

    void Update()
    {
        Last_Zombie();
        OnKey();
    }
    // 적 생성
    void CreateEnemy()
    {
        int i = Random.Range(0, Enemys.Length);
        //적 프리팹 생성
        GameObject clone = Instantiate(Enemys[i]);
        clone.GetComponent<Enemy>().Init(transform.position);
        ZombieCount++;
        Debug.Log(ZombieCount);
    }

    void Last_Zombie()
    {
        if (MaxZombie == ZombieCount)
        {
            IsKey = true;
            ZombieCount++;
        }
    }
    void OnKey()
    {
        if (IsKey)
        {
            PosX = Random.Range(0.0f, 35.0f);
            PosZ = Random.Range(0.0f, 35.0f);
            GameObject clone = Instantiate(Key);
            clone.transform.position = new Vector3(PosX, 2.0f, PosZ);
            IsKey = false;
        }
    }
}
