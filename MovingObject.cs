using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * MovingObject
 * ĳ���Ͱ� �����̱� ���� ����� �����Ѵ�. ĳ������ vector������ speed, boxcollider, audio, animator �� ���� ��ҵ��� ����Ѵ�. 
 */

public class MovingObject : MonoBehaviour
{
    public string characterName; //ĳ�����̸��� ��´�.

    /* ���带 ������ �����Ϸ��� ������� �ִ�. -> AudioManager.cs�� ���� */
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

    private Vector3 vector; //3���� ���� ���ÿ� ������ ����, Vector3. (ĳ���� ��ǥ ��)

    public int walkCount; //�ȼ� ������ �ƴ� Ÿ�� ������ �̵�
    private int currentWalkCount; // while���� index ����
    public float speed;
    /*
    public float runSpeed;
    private float applyRunSpeed; //shiftŰ�� �������� �� ���� speed�� runSpeed�� ���Ѵ�.
    private bool applyRunFlag = false; //�� ĭ�� �̵��ϴ� ���� �����Ѵ�.
    private bool canMove = true; //�ڷ�ƾ ���� ����
    */
    private bool notCoroutine = false; //coroutine ���� ���� ���� ���� (false : ���� X, true : ���� O) 

    private BoxCollider2D boxCollider; //ĳ������ �̵� ������ ���� ����
    public LayerMask layerMask; //����� �Ұ����� Layer ���� �� Layer ����
    private Animator animator;
    public Queue<string> queue; //2���� ������ �ذ��ϱ� ���� ��� (1. �ѹ��� ���Ƽ� �����̴� ��, 2. Standing ����� �����Ǵ� ��)

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>(); // ������ BoxCollider Component ���� : ĳ���� �̵� ���� ����
        /*���带 ������ �����Ϸ��� ������� �ִ�. -> AudioManager.cs�� ����.
         * audioSource = GetComponent<AudioSource>(); //������ AudioSource Component ���� : �����
         */
        theAudio = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>(); // ������ Animator Component ���� : ĳ���� �̵� �ִϸ��̼� ����
    }

    public void Move(string _dir, int _frequency = 5)
    {
        queue.Enqueue(_dir);//_dir�� �ִ� ������ ť�� �ִ´�.
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }

    }

    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while (queue.Count != 0) //ť�� ����������� �ݺ�
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

            string direction = queue.Dequeue(); //������ ������ �̾� direction�� ����
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
                bool checkCollsionFlag = CheckCollsion(); //ĳ���ͳ��� �浹�� true  //ĳ���Ϳ� ������ boxcollider�� �浹���� Ȯ��
                if (checkCollsionFlag)
                {
                    animator.SetBool("Walking", false); //�浹�� ����
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    break; //�浹�� ���� ��� ��� ����
                }
            }

            animator.SetBool("Walking", true);

            //AudioManger���� ���ϴ�(����) ���带 Play�Ѵ�.
            int temp = Random.Range(1, 4); // ���� �� ���� �غ�
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

            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount); //boxCollider�� �����̷��� �������� �̸� ������ ĳ���� ���ӹ���

            while (currentWalkCount < walkCount)
            {
                transform.Translate(vector.x * speed, vector.y * speed, 0); //Translate �����ִ� ������ ��ġ��ŭ ����
                currentWalkCount++;
                if (currentWalkCount == 12)
                    boxCollider.offset = Vector2.zero; //�ٽ� ����ġ
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
    IEnumerator MoveCoroutine() { // �ڷ�ƾ�� �����ϴ� �޼ҵ�� �ڷ�ƾ�� ���ÿ� ����ȴ�. (��ġ ����ó��ó�� ������ �����ϴ�.)

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

            animator.SetFloat("DirX", vector.x); //vector.x�� ���� ���� animator ����
            animator.SetFloat("DirY", vector.y);

            RaycastHit2D hit;
            Vector2 start = transform.position; // (A����) ĳ���� ����ġ��
            Vector2 end =  start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount) ; // (B����) ĳ���Ͱ� �̵��ϰ����ϴ� ��ġ��
            boxCollider.enabled = false; // ĳ���� ��ü�� boxCollider�� �������� �浹�ϴ� ���� �����ϱ� ���ؼ� �Ͻ������� boxCollider�� ����.
            hit = Physics2D.Linecast(start, end, layerMask);  // A �������� B �������� �������� �߻��Ѵٰ� ������ ��, ������ ������ ��� hit: Null, ���ع��� �浹�� ��� hit: �ش� ���ع�.
            boxCollider.enabled = true; // ������ �߻� �� ���󺹱�

            if (hit.transform != null) { //���ع��� �ִٸ�, ���� �ȴ� ����� ��� �׸��д�.
                break;
            }

            animator.SetBool("Walking", true); //animator ���� ��ȯ (standing tree -> walking tree)

            //AudioManger���� ���ϴ�(����) ���带 Play�Ѵ�.
            int temp = Random.Range(1, 4); // ���� �� ���� �غ�
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
                { //�¿�
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0); //�¿� �̵��̹Ƿ� vector.x�� ���� 1 �Ǵ� -1. (���� speed�� ��ȣ�� �����Ѵ�.)
                }
                else if (vector.y != 0)
                { //����
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0); //Translate �޼ҵ�: ����ġ���� �ش� ����ŭ �����ش�.
                    //�ٸ� ������δ�...
                    //tranform.position = vector;
                    //..
                    //
                }

                if (applyRunFlag)
                {
                    currentWalkCount++;
                }
                currentWalkCount++;

                yield return new WaitForSeconds(0.01f); // 0.01�� ���

                // ���带 ������ �����Ϸ��� ������� �ִ�. -> AudioManager.cs�� ����.
                //
                //if (currentWalkCount % 9 == 2) { // 20ȸ �ݺ� �� 2�� �����ϱ� ���� 9�� ���� (9, 18). 
                //    int temp = Random.Range(1, 2); // ���� �� ���� �غ�
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
        animator.SetBool("Walking", false); //animator ���� ��ȯ (walking tree -> standing tree)
        canMove = true;
    }*/

    /*
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {         
            //Key Input (Ű �Է� ó���� �� �����Ӹ��� �̷������ϱ� ������ Update �Ͽ� �ۼ�
            if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0)) // horizontal: ��(return-1)��(return1) ����Ű / vertical: ��(return1)��(return-1) ����Ű 
            {
                canMove = false; //�ڷ�ƾ�� �ѹ��� ����� �� �ֵ���
                StartCoroutine(MoveCoroutine());
            }
        }
        
    }
    */
}
