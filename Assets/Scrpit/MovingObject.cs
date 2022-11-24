using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

    public string characterName; //캐릭터이름을 담는다.

    public float speed;
    public int walkCount;
    protected int currentWalkCount;

    private bool notCoroutine = false; //coroutine의 동시실행 방지 / false : 실행 X, true : 실행 중 
    protected Vector3 vector;

    public Queue<string> queue; //2가지 오류를 해결하기 위해 사용 1. 한번에 몰아서 움직임 방지 2. Standing 모션이 생략됨

    public BoxCollider2D boxCollider;
    public LayerMask layerMask;
    public Animator animator;

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
        while(queue.Count != 0) //큐가 비워질때까지 반복
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

}
