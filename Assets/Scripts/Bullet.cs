/*
 * Office Zombie - Bullet.cs
 * 총알의 속성 및 기능 클래스
 * 20170807 이학성
 * 20170808 : 총알팅기는 효과 matrial로 변경, 총알 삭제 시간 추가, find이용해서 point찾기
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum COLOR { NONE = 0, RED, GREEN, BLUE, DISAPPER }
public class Bullet : MonoBehaviour {
    //좀비 사망시 파티클 생성
    [SerializeField] GameObject ZombieFire;
    [SerializeField] AudioClip[] clips;

    AudioSource audioSource;
    //총알 속도
    float speed = 1000.0f;
    //박스로 들어간 총알인지 확인
    private bool Isbox = false;
    //총알 삭제를 위한 시간조정 변수
    private float Destroy_Time = 0.0f;
    //find를 위한
    public GameObject bulletpoint;
    //총알 충돌 횟수
    COLOR Crash_Count;

    void Awake()
    {
        Crash_Count = COLOR.NONE;
        audioSource = GetComponent<AudioSource>();
        //총알 쏴줌
        bulletpoint = GameObject.Find("BulletPoint");
        GetComponent<Rigidbody>().AddForce(bulletpoint.transform.forward * speed);
    }
    void Update()
    {
        LifeTime();
        Crash_Color();
    }
    //총알과 충돌시
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("FIELD"))
        {
            PlaySound(0);
            GetComponent<Rigidbody>().AddForce(bulletpoint.transform.forward * speed);
            Crash_Count++;
        }
        if (col.gameObject.tag.Equals("ENEMY_RED") && Crash_Count == COLOR.RED)
        {
            KillZomble();
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag.Equals("ENEMY_GREEN") && Crash_Count == COLOR.GREEN)
        {
            KillZomble();
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag.Equals("ENEMY_BLUE") && Crash_Count == COLOR.BLUE)
        {
            KillZomble();
            Destroy(col.gameObject);
        }
    }
    //총알의 존재 시간
    void LifeTime()
    {
        if (Destroy_Time > 2.0f)
        {
            Destroy(gameObject);
            Destroy_Time = 0;
        }
        else
        {
            Destroy_Time += 1.0f * Time.deltaTime;
        }
    }
    //반사된 횟수 판단
    void Crash_Color()
    {
        switch (Crash_Count)
        {
            case COLOR.RED:
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case COLOR.GREEN:
                gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case COLOR.BLUE:
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            /*case COLOR.DISAPPER:
                Destroy(gameObject);
                break;
                */
        }
    }
    void KillZomble()
    {
        PlaySound(1);
        GameObject clone = Instantiate(ZombieFire);
        clone.transform.position = transform.position;
    }
    //사운드 재생
    void PlaySound(int _idx)
    {
        audioSource.Stop();
        audioSource.clip = clips[_idx];
        audioSource.PlayOneShot(clips[_idx]);
    }
}
