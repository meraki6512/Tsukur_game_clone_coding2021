using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * ChoiceManager
 * ���ϴ� ����Ʈ�� �������� �����ϰ� �̸� ����ڰ� ������ �� �ֵ��� ����.
 */

public class ChoiceManager : MonoBehaviour
{
    #region Singleton
    //�̱���ȭ(only one exists)�� ���� �ڱ� �ڽ��� ��ü�� ������ static ������ �����Ѵ�.
    static public ChoiceManager instance;

    //�̱���ȭ ���� (Scene �̵� ��, ChoiceManager Destroy�� ���´�.)
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    #endregion Singleton

    // ���� ���
    private AudioManager theAudio;
    public string keySound;
    public string enterSound;

    // Choice ��ü�� question�� answer �迭 ��ü�� ��Ƶ� ����.
    private string question;
    private List<string> answerList;

    // Text
    public Text questionText;
    public Text[] answerText;

    // ��ҿ� ��Ȱ��ȭ��ų �������� ����. (setActive)
    public GameObject go;
    // ���õ� answer ���� Panel(���� ��) ���� ����.
    public GameObject[] answerPanel; 

    // Animator
    public Animator anim;

    // ��� (!choiceIng �̿�)
    public bool choiceIng;
    // Ű event(���� ��) ó�� Ȱ��ȭ ����
    private bool keyInput;

    // �迭�� ũ�� (answer ���� â ����)
    private int count;
    // ���� ��� (���� â)
    private int result;

    // Coroutine���� ����� ����
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>(); // ����� �Ŵ���
        answerList = new List<string>(); // answer ����Ʈ �ʱ�ȭ
        //text �ʱ�ȭ
        questionText.text = ""; 
        for (int i = 0; i < answerText.Length; i++)
        {
            answerText[i].text = "";
            answerPanel[i].setActive(false);
        }
    }

    // ���� â ��
    public void ShowChoice(Choice _choice) {
        go.SetActive(true);
        choiceIng = true;
        result = 0; // ���õ� ������ �ʱ�ȭ
        question = _choice.question;
        for (int i = 0; i < _choice.answer.Length; i++) {
            answerList.Add(_choice.answer[i]);
            answerPanel[i].setActive(true);
            count = i; 
        }
        anim.SetBool("Appear", true);
        Selection(); // ���õ� ������ ���� �ʱ�ȭ
        StartCoroutine(ChoiceCoroutine());
    }

    // choice ���� : questionText, answerText, answerPanel, answerList, anim, choiceIng �ʱ�ȭ
    public void ExitChoice() {
        questionText.text = "";
        for (int i = 0; i < count; i++) {
            answerText[i].text = "";
            answerPanel[i].setActive(false);
        }
        answerList.Clear();
        anim.SetBool("Appear", false);
        choiceIng = false;
        StartCoroutine(ChoiceExitCoroutine());
        go.SetActive(false);
    }

    public int GetResult() {
        return result;
    }

    /*
     * Choice & ChoiceExit Coroutine
     * - Choice Animator ��� (appear & dissapear)
     * - Question & Answer Courtine ����
     * - keyInput �� �ο�
     */

    IEnumerator ChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.2f); // choiceAppear Animator�� ����� ���� ��� ��ٸ���.

        StartCoroutine(TypingQuestion()); // ������ ����.
        StartCoroutine(TypingAnswer0()); // ������ ����.
        //answerList �迭 ũ�⿡ ���� ������ ����.
        if (count >= 2)
            StartCoroutine(TypingAnswer1());
        if (count >= 3)
            StartCoroutine(TypingAnswer2());
        if (count >= 4)
            StartCoroutine(TypingAnswer3());

        yield return new WaitForSeconds(0.5f);
        keyInput = true;
    }

    IEnumerator ChoiceExitCoroutine()
    {
        yield return new WaitForSeconds(0.2f); // choiceDisappear Animator�� ����� ���� ��� ��ٸ���.
    }

    /*
     * Coroutine�� �ϳ��� ������ Text ����� �����ϸ�, ��� Text�� �ϳ��� Coroutine �ȿ��� ���ÿ� �۵��Ͽ� �߸��� �並 �����ֱ� ������
     * Question�� AnswerList�� Coroutine�� ��� ���� �����.
     */

    IEnumerator TypingQuestion() {
        for (int i = 0; i < questionText.Length; i++) {
            questionText.text += question[i];
            yield return waitTime;
        }
        
    }

    IEnumerator TypingAnswer0()
    {
        yield return new WaitForSeconds(0.4f); //�ĵ�Ÿ�� ����
        for (int i = 0; i < answerText[0].Length; i++)
        {
            answerText[0].text += answerText[0][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer1()
    {
        yield return new WaitForSeconds(0.5f); //�ĵ�Ÿ�� ����
        for (int i = 0; i < answerText[1].Length; i++)
        {
            answerText[1].text += answerText[1][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer2()
    {
        yield return new WaitForSeconds(0.6f); //�ĵ�Ÿ�� ����
        for (int i = 0; i < answerText[2].Length; i++)
        {
            answerText[2].text += answerText[2][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer3()
    {
        yield return new WaitForSeconds(0.7f); //�ĵ�Ÿ�� ����
        for (int i = 0; i < answerText[3].Length; i++)
        {
            answerText[3].text += answerText[3][i];
            yield return waitTime;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (keyInput) {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                theAudio.Play(keySound);
                if (result > 0)
                {
                    result--;
                }
                else
                {
                    result = count; //0���� �۾�����, Ű ��ġ rotate
                }
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                theAudio.Play(keySound);
                if (result < count)
                {
                    result++;
                }
                else
                {
                    result = 0; //�迭 ũ�⺸�� Ŀ����, Ű ��ġ rotate
                }
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.Z)) {
                theAudio.Play(enterSound);
                keyInput = false;
                ExitChoice(); 
            }

        }

    }

    // ���õ� answer â�� (������ ������) �� ������ ���� �޼ҵ�.
    public void Selection() {
        Color color = answerPanel[0].GetComponent<Image>().color; //�г� �̹����� color�� �����´�.
        color.a = 0.75f; //alpha(����)�� 0.75f�� �����Ѵ�.
        for (int i = 0; i <= count; i++) {
            answerPanel[i].GetComponent<Image>().color = color;
        }
        color.a = 1f;
        answerPanel[result].GetComponent<Image>().color = color; //���õ� answer(result)�� color alpha 1f�� ������.
    }

}