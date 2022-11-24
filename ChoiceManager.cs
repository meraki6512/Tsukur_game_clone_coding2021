using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * ChoiceManager
 * 원하는 포인트에 선택지를 생성하고 이를 사용자가 선택할 수 있도록 구현.
 */

public class ChoiceManager : MonoBehaviour
{
    #region Singleton
    //싱글턴화(only one exists)를 위한 자기 자신을 객체로 가지는 static 변수를 생성한다.
    static public ChoiceManager instance;

    //싱글턴화 과정 (Scene 이동 시, ChoiceManager Destroy를 막는다.)
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

    // 사운드 재생
    private AudioManager theAudio;
    public string keySound;
    public string enterSound;

    // Choice 객체의 question과 answer 배열 객체를 담아둘 변수.
    private string question;
    private List<string> answerList;

    // Text
    public Text questionText;
    public Text[] answerText;

    // 평소에 비활성화시킬 목적으로 선언. (setActive)
    public GameObject go;
    // 선택된 answer 하의 Panel(투명도 등) 관리 목적.
    public GameObject[] answerPanel; 

    // Animator
    public Animator anim;

    // 대기 (!choiceIng 이용)
    public bool choiceIng;
    // 키 event(사운드 등) 처리 활성화 여부
    private bool keyInput;

    // 배열의 크기 (answer 선택 창 개수)
    private int count;
    // 선택 결과 (선택 창)
    private int result;

    // Coroutine에서 사용할 변수
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>(); // 오디오 매니저
        answerList = new List<string>(); // answer 리스트 초기화
        //text 초기화
        questionText.text = ""; 
        for (int i = 0; i < answerText.Length; i++)
        {
            answerText[i].text = "";
            answerPanel[i].setActive(false);
        }
    }

    // 선택 창 뷰
    public void ShowChoice(Choice _choice) {
        go.SetActive(true);
        choiceIng = true;
        result = 0; // 선택된 선택지 초기화
        question = _choice.question;
        for (int i = 0; i < _choice.answer.Length; i++) {
            answerList.Add(_choice.answer[i]);
            answerPanel[i].setActive(true);
            count = i; 
        }
        anim.SetBool("Appear", true);
        Selection(); // 선택된 선택지 연출 초기화
        StartCoroutine(ChoiceCoroutine());
    }

    // choice 종료 : questionText, answerText, answerPanel, answerList, anim, choiceIng 초기화
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
     * - Choice Animator 대기 (appear & dissapear)
     * - Question & Answer Courtine 실행
     * - keyInput 값 부여
     */

    IEnumerator ChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.2f); // choiceAppear Animator가 실행될 동안 잠깐 기다린다.

        StartCoroutine(TypingQuestion()); // 무조건 실행.
        StartCoroutine(TypingAnswer0()); // 무조건 실행.
        //answerList 배열 크기에 따라 선택적 실행.
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
        yield return new WaitForSeconds(0.2f); // choiceDisappear Animator가 실행될 동안 잠깐 기다린다.
    }

    /*
     * Coroutine을 하나만 생성해 Text 출력을 관리하면, 모든 Text가 하나의 Coroutine 안에서 동시에 작동하여 잘못된 뷰를 보여주기 때문에
     * Question과 AnswerList의 Coroutine을 모두 따로 만든다.
     */

    IEnumerator TypingQuestion() {
        for (int i = 0; i < questionText.Length; i++) {
            questionText.text += question[i];
            yield return waitTime;
        }
        
    }

    IEnumerator TypingAnswer0()
    {
        yield return new WaitForSeconds(0.4f); //파도타기 연출
        for (int i = 0; i < answerText[0].Length; i++)
        {
            answerText[0].text += answerText[0][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer1()
    {
        yield return new WaitForSeconds(0.5f); //파도타기 연출
        for (int i = 0; i < answerText[1].Length; i++)
        {
            answerText[1].text += answerText[1][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer2()
    {
        yield return new WaitForSeconds(0.6f); //파도타기 연출
        for (int i = 0; i < answerText[2].Length; i++)
        {
            answerText[2].text += answerText[2][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer3()
    {
        yield return new WaitForSeconds(0.7f); //파도타기 연출
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
                    result = count; //0보다 작아지면, 키 위치 rotate
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
                    result = 0; //배열 크기보다 커지면, 키 위치 rotate
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

    // 선택된 answer 창의 (투명도를 조절한) 뷰 연출을 위한 메소드.
    public void Selection() {
        Color color = answerPanel[0].GetComponent<Image>().color; //패널 이미지의 color를 가져온다.
        color.a = 0.75f; //alpha(투명도)를 0.75f로 조절한다.
        for (int i = 0; i <= count; i++) {
            answerPanel[i].GetComponent<Image>().color = color;
        }
        color.a = 1f;
        answerPanel[result].GetComponent<Image>().color = color; //선택된 answer(result)만 color alpha 1f로 재조절.
    }

}