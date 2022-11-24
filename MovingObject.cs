using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * MovingObject
 * 캐릭터가 움직이기 위한 기능을 구현한다. 캐릭터의 vector값부터 speed, boxcollider, audio, animator 등 여러 요소들을 고려한다. 
 */

public class MovingObject : MonoBehaviour
{
    public string characterName; //캐릭터이름을 담는다.

    /* 사운드를 일일이 관리하려면 어려움이 있다. -> AudioManager.cs로 관리 */
    /*
    public AudioClip walkSound_1; //Sound File
    public AudioClip walkSound_2; //Sound File
    private AudioSource audioSource; //Sound Player
    */
    public string walkSounds_1;
    public string walkSounds_2;
    public string walkSounds_3;
    public string walkSounds_4;
    private AudioManager theAudio;

    private Vector3 vector; //3개의 값을 동시에 가지는 변수, Vector3. (캐릭터 좌표 등)

    public int walkCount; //픽셀 단위가 아닌 타일 단위로 이동
    private int currentWalkCount; // while문의 index 역할
    public float speed;
    /*
    public float runSpeed;
    private float applyRunSpeed; //shift키가 눌러졌을 때 원래 speed에 runSpeed를 더한다.
    private bool applyRunFlag = false; //두 칸씩 이동하는 것을 방지한다.
    private bool canMove = true; //코루틴 실행 제어
    */
    private bool notCoroutine = false; //coroutine 동시 실행 방지 목적 (false : 실행 X, true : 실행 O) 

    private BoxCollider2D boxCollider; //캐릭터의 이동 제한을 위한 변수
    public LayerMask layerMask; //통과가 불가능한 Layer 설정 등 Layer 구분
    private Animator animator;
    public Queue<string> queue; //2가지 오류를 해결하기 위해 사용 (1. 한번에 몰아서 움직이는 것, 2. Standing 모션이 생략되는 것)

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>(); // 변수에 BoxCollider Component 연결 : 캐릭터 이동 범위 제한
        /*사운드를 일일이 관리하려면 어려움이 있다. -> AudioManager.cs로 관리.
         * audioSource = GetComponent<AudioSource>(); //변수에 AudioSource Component 연결 : 오디오
         */
        theAudio = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>(); // 변수에 Animator Component 연결 : 캐릭터 이동 애니메이션 구현
    }

    public void Move(string _dir, int _frequency = 5)
    {
        queue.Enqueue(_dir);//_dir에 있는 방향을 큐에 넣는다.
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }

    }

    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while (queue.Count != 0) //큐가 비워질때까지 반복
        {
            switch (_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f);
                    break;
                case 2:
                    yield return new WaitForSeconds(3f);
                    break;
                case 3:
                    yield return new WaitForSeconds(2f);
                    break;
                case 4:
                    yield return new WaitForSeconds(1f);
                    break;
                case 5:
                    break;
            }

            string direction = queue.Dequeue(); //움직일 방향을 뽑아 direction에 저장
            vector.Set(0, 0, vector.z);

            switch (direction)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIGHT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = -1f;
                    break;
            }

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            while (true)
            {
                bool checkCollsionFlag = CheckCollsion(); //캐릭터끼리 충돌시 true  //캐릭터에 설정된 boxcollider로 충돌여부 확인
                if (checkCollsionFlag)
                {
                    animator.SetBool("Walking", false); //충돌시 정지
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    break; //충돌이 없을 경우 계속 진행
                }
            }

            animator.SetBool("Walking", true);

            //AudioManger에서 원하는(랜덤) 사운드를 Play한다.
            int temp = Random.Range(1, 4); // 사운드 네 가지 준비
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSounds_1);
                    break;
                case 2:
                    theAudio.Play(walkSounds_2);
                    break;
                case 3:
                    theAudio.Play(walkSounds_3);
                    break;
                case 4:
                    theAudio.Play(walkSounds_4);
                    break;
            }

            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount); //boxCollider을 움직이려는 방향으로 미리 움직여 캐릭터 끼임방지

            while (currentWalkCount < walkCount)
            {
                transform.Translate(vector.x * speed, vector.y * speed, 0); //Translate 현재있는 값에서 수치만큼 더함
                currentWalkCount++;
                if (currentWalkCount == 12)
                    boxCollider.offset = Vector2.zero; //다시 원위치
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
            if (_frequency != 5)
                animator.SetBool("Walking", false);
        }
        animator.SetBool("Walking", false);
        notCoroutine = false;
    }

    protected bool CheckCollsion()
    {
        RaycastHit2D hit;

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;

        if (hit.transform != null)
            return true;
        return false;
    }

    /*
    IEnumerator MoveCoroutine() { // 코루틴을 실행하는 메소드와 코루틴이 동시에 진행된다. (마치 다중처리처럼 구현이 가능하다.)

        while ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            if (vector.x != 0)
                vector.y = 0;

            animator.SetFloat("DirX", vector.x); //vector.x의 값에 따라 animator 설정
            animator.SetFloat("DirY", vector.y);

            RaycastHit2D hit;
            Vector2 start = transform.position; // (A지점) 캐릭터 현위치값
            Vector2 end =  start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount) ; // (B지점) 캐릭터가 이동하고자하는 위치값
            boxCollider.enabled = false; // 캐릭터 자체의 boxCollider에 레이저가 충돌하는 것을 방지하기 위해서 일시적으로 boxCollider를 끈다.
            hit = Physics2D.Linecast(start, end, layerMask);  // A 지점에서 B 지점까지 레이저를 발사한다고 가정할 때, 무사히 도착할 경우 hit: Null, 방해물에 충돌할 경우 hit: 해당 방해물.
            boxCollider.enabled = true; // 레이저 발사 후 원상복귀

            if (hit.transform != null) { //방해물이 있다면, 이후 걷는 모션을 모두 그만둔다.
                break;
            }

            animator.SetBool("Walking", true); //animator 상태 전환 (standing tree -> walking tree)

            //AudioManger에서 원하는(랜덤) 사운드를 Play한다.
            int temp = Random.Range(1, 4); // 사운드 네 가지 준비
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSounds_1);
                    break;
                case 2:
                    theAudio.Play(walkSounds_2);
                    break;
                case 3:
                    theAudio.Play(walkSounds_3);
                    break;
                case 4:
                    theAudio.Play(walkSounds_4);
                    break;
            }
            //theAudio.SetVolume(walkSounds_2, 0.5f); //test

            while (currentWalkCount < walkCount)
            {
                if (vector.x != 0)
                { //좌우
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0); //좌우 이동이므로 vector.x의 값은 1 또는 -1. (따라서 speed의 부호만 결정한다.)
                }
                else if (vector.y != 0)
                { //상하
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0); //Translate 메소드: 현위치에서 해당 값만큼 더해준다.
                    //다른 방법으로는...
                    //tranform.position = vector;
                    //..
                    //
                }

                if (applyRunFlag)
                {
                    currentWalkCount++;
                }
                currentWalkCount++;

                yield return new WaitForSeconds(0.01f); // 0.01초 대기

                // 사운드를 일일이 관리하려면 어려움이 있다. -> AudioManager.cs로 관리.
                //
                //if (currentWalkCount % 9 == 2) { // 20회 반복 중 2번 실행하기 위해 9로 나눔 (9, 18). 
                //    int temp = Random.Range(1, 2); // 사운드 두 가지 준비
                //    switch (temp) {
                //        case 1:
                //            audioSource.clip = walkSound_1;
                //            audioSource.Play();
                //            break;
                //        case 2:
                //            audioSource.clip = walkSound_2;
                //            audioSource.Play();
                //            break;
                //    }
                //}
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false); //animator 상태 전환 (walking tree -> standing tree)
        canMove = true;
    }*/

    /*
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {         
            //Key Input (키 입력 처리는 매 프레임마다 이뤄져야하기 때문에 Update 하에 작성
            if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0)) // horizontal: 좌(return-1)우(return1) 방향키 / vertical: 상(return1)하(return-1) 방향키 
            {
                canMove = false; //코루틴이 한번씩 실행될 수 있도록
                StartCoroutine(MoveCoroutine());
            }
        }
        
    }
    */
}
