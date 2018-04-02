/*
 * Office Zombie - Character.cs
 * 캐릭터 속성 및 기능 스크립트
 * 부모 클래스 : Entity 
 * 20170804 이학성
 * 20170807 총알의 기능 따로 뺌
 * 20170809 UI추가(박영균)
 * 이동 및 공격
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Character : Entity {
    //총알 프리팹
    [SerializeField] GameObject bulletPrefab;
    //총알 나가는 포인트
    [SerializeField] Transform  bulletPoint;
    //목숨 UI
    [SerializeField] GameObject[] lifeObjects;
    //탄 갯수 UI
    [SerializeField] GameObject[] bulletObjects;
    //총알갯수 UI
    [SerializeField] Text textFireCount;
    //피격효과
    [SerializeField] Image bloodScreen;
    //사운드소스
    [SerializeField] AudioClip[] clips;

    //점프를 주는 힘
    readonly float jumpForce = 8.0f;
    //점프 후 떨어지는 중력값
    //readonly float gravity = -20.0f;
    //총알 딜레이
    readonly float fireDelay = 0.1f;
    //총알의 최대값
    readonly int maxFireCount = 30;

    
    public bool isHit;
    //UI 카운팅
    private int lifeCount;
    private int bulletCount;
    //키 있는지
    private bool IsKey = false;
    //현재 총알갯수
    private int curFireCount;
    //마우스 이동
    float xSpeed, ySpeed;
	float yMinLimit, yMaxLimit;
	float x,y;
    //총알 나가는 힘
    float buletPower = 500f;

    AudioSource audioSource;
    Quaternion rotation;
    CharacterController controller;

	void Awake(){
		//백그라운드 실행
		Application.runInBackground = true;

        //커서보이지 않고 위치고정
		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Init();
        //회전값
        x = transform.eulerAngles.y;
        y = transform.eulerAngles.x;
        rotation = Quaternion.Euler(y, x, 0.0f);

        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
	}
    void Update()
    {
        Update_Inputs();
        if(bloodScreen.color.a > 0.0f)
        {
            Color color = bloodScreen.color;
            color.a -= 0.02f;
            bloodScreen.color = color;
        }
    }
    //회전 값 제한
    float ClampAngle(float _angle, float _min, float _max)
    {
        if (_angle < -360)
            _angle += 360;
        if (_angle > 360)
            _angle -= 360;

        return Mathf.Clamp(_angle, _min, _max);
    }
    //초기셋팅
    void Init()
    {
        //캐릭터
        moveSpeed = 3.0f;
        xSpeed = 250.0f;
        ySpeed = 120.0f;
        yMinLimit = -80.0f;
        yMaxLimit = 30.0f;
        //UI
        lifeCount = 3;
        bulletCount = 3;
        curFireCount = 30;
        textFireCount.text = curFireCount + "/" + maxFireCount;
        isHit = false;
    }
    //키 조작
    void Update_Inputs()
    {
        //캐릭터 이동값
        float xPos = Input.GetAxisRaw("Horizontal");
        float zPos = Input.GetAxisRaw("Vertical");

        //유저가 땅에 있는가?
        if (controller.isGrounded)
        {
            //이동 방향
            moveDirection = rotation * new Vector3(xPos, 0.0f, zPos) * moveSpeed;
            //점프
            if (Input.GetKeyDown(KeyCode.Space))
                moveDirection.y = jumpForce;
        }
        //땅이 아니면 땅 방향으로 떨어트림
        else
            moveDirection.y += gravity * Time.deltaTime;
        //이동
        controller.Move(moveDirection * Time.deltaTime);

        //카메라 회전값 값
        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        //회전값 계산
        rotation = Quaternion.Euler(y, x, 0.0f);
        //회전
        transform.rotation = rotation;
        //마우스 좌측클릭시
        if (Input.GetMouseButtonDown(0))
        {
            if (curFireCount > 0)
            {
                GameObject clone = Instantiate(bulletPrefab);
                //방향과 각도 총알 포인트로
                clone.transform.position = bulletPoint.position;
                clone.transform.rotation = bulletPoint.rotation;

                curFireCount--;
                textFireCount.text = curFireCount + "/" + maxFireCount;
            }
            //불릿 확인
            else if (bulletCount > 0)
            {
                Bullet_Odd();
                curFireCount = 30;
                textFireCount.text = curFireCount + "/" + maxFireCount;
            }
            else if (bulletCount <= 0 && curFireCount == 0)
            {

            }
        }
    }
    //충돌
    private void OnTriggerEnter(Collider other)
    {
        //적
        if (other.tag.Contains("ENEMY"))
        {
            Life_Odd();
            PlaySound(0);
            Destroy(other.gameObject);
        }
        //생명 아이템
        else if (other.tag.Equals("HEART"))
        {
            PlaySound(1);
            Destroy(other.gameObject);
            Life_Recovery();

        }
        //총알 아이템
        else if (other.tag.Equals("MAGAZINE"))
        {
            PlaySound(2);
            Destroy(other.gameObject);
            Bullet_Recovery();
        }
        else if (other.tag.Equals("KEY"))
        {
            Destroy(other.gameObject);
            IsKey = true;
        }
        else if (other.tag.Equals("EXIT"))
        {
            if (IsKey)
            {
                Debug.Log("EXIT");
                //SceneManager.LoadScene("");
            }
        }
    }
    //사운드 재생
    void PlaySound(int _idx)
    {
        audioSource.Stop();
        audioSource.clip = clips[_idx];
        audioSource.PlayOneShot(clips[_idx]);
    }


    //UI - 박영균
    public void Life_Odd()
    {
        if (lifeCount > 0)
        {
            lifeObjects[lifeCount - 1].gameObject.SetActive(false);
            lifeCount--;
        }else if(lifeCount <= 0)
        {
            //SceneManager.LoadScene("");
        }
        Color color = bloodScreen.color;
        color.a = 1.0f;
        bloodScreen.color = color;
    }

    void Bullet_Odd()
    {
        if (bulletCount > 0)
        {
            bulletObjects[bulletCount - 1].gameObject.SetActive(false);
            bulletCount--;
        }
    }

    void Bullet_Recovery()
    {
        if (bulletCount < 3)
        {
            bulletObjects[bulletCount].gameObject.SetActive(true);
            bulletCount++;
        }
    }

    void Life_Recovery()
    {
        if (lifeCount < 3)
        {
            lifeObjects[lifeCount].gameObject.SetActive(true);
            lifeCount++;
        }
    }
}
