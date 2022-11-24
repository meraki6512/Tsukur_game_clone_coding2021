using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NumberSystem
 * 다이얼 시스템을 활용하고 싶은 이벤트에서 object를 만들어 NumberSystem class를 이용할 수 있다. 
 */

public class NumberSystem : MonoBehaviour
{
    // 사운드 : 방향키, 결정키, 취소키 등 이벤트
    private AudioManager theAudio;
    public string keySound;
    public string enterSound;
    public string cancelSound;
    public string correctSound;

    private int count; // 배열의 크기. 즉, 자릿수를 의미한다.
    private int selectedTextBox; // 선택된 자릿수. 즉, 어느 박스가 선택되었는지를 의미한다. 
    private int result; // 플레이어가 선택한 값.
    private int correctNumber; //정답.

    private string tempNumber;

    public GameObject superObject; // 다이얼 object의 위치를 (자릿수에 따라) 결정한다.
    public int position; //inspector창에서 30으로 설정하면 적당한 위치에 온다.
    public GameObject[] panel;
    public Text[] numberText;

    public Animator anim;

    public bool activated; // 대기 (return new waitUntil)
    public bool keyInput; // 키처리 활성화 설정
    public bool correctFlag; // 정답 여부 (result==correctNumber?)

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>(); 
    }

    // 다이얼 화면 뷰. (활성화)
    public void ShowNumber(int _correctNumber) 
    {
        //초기화
        correctNumber = _correctNumber;
        activated = true;
        correctFlag = false;
        string temp = correctNumber.ToString(); //자릿수 구할 때, Length 기능을 이용하기 위함.
        for (int i = 0; i < temp.Length; i++) {
            count = i; //count==temp.Lenght-1
            panel[i].SetActive(true);
            numberText[i].text = "0";
        }

        //뷰 위치 조정
        superObject.transform.position = new Vector3(superObject.transform.position.x + position*count, 
                                                     superObject.transform.position.y, 
                                                     superObject.transform.position.z);
        //초기화
        selectedTextBox = 0;
        result = 0;
        SetColor(); 
        anim.SetBool("Appear", true);
        keyInput = true;
    }

    //결과(정답 여부) 반환
    public bool GetResult()
    {
        return correctFlag;
    }

    //Update 메소드 내의 keyInput(UP&DOWN) 처리를 위한 메소드.
    public void SetNumber(string _arrow)
    {
        int temp = int.Parse(numberText[selectedTextBox].text); //numberText[selectedTextBox].text를 Integer 형으로 강제 형변환. (숫자 계산을 위해)
        if (_arrow == "DOWN")
        {
            if (temp == 0)
            {
                temp = 9; //down 키처리 중 number값이 0이 되면, 9로 바꿔준다.
            }
            else
            {
                temp--;
            }
        }
        else if (_arrow == "UP")
        {
            if (temp == 9)
            {
                temp = 0; //up 키처리 중 number값이 9이 되면, 0로 바꿔준다.
            }
            else
            {
                temp++;
            }
        }
        numberText[selectedTextBox].text = temp.ToString(); //다시 String 형식으로 변환해준다.
    }

    public void SetColor()
    {
        Color color = numberText[0].color; //패널 이미지의 color를 가져온다.
        color.a = 0.3f; //alpha(투명도)를 0.3f로 조절한다.
        for (int i = 0; i <= count; i++)
        {
            numberText[i].color = color;
        }
        color.a = 1f;
        numberText[selectedTextBox].color = color; //선택된 answer(result)만 color alpha 1f로 재조절.
    }

    // Update is called once per frame
    void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                theAudio.Play(keySound);
                SetNumber("UP");
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                theAudio.Play(keySound);
                SetNumber("DOWN");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                theAudio.Play(keySound);
                if (selectedTextBox < count)
                {
                    selectedTextBox++;
                }
                else
                {
                    selectedTextBox = 0; //count의 자릿수에서 left키 입력 시, rotate.
                }
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                theAudio.Play(keySound);
                if (selectedTextBox > 0)
                {
                    selectedTextBox--;
                }
                else
                {
                    selectedTextBox = count; //0의 자릿수에서 right키 입력 시, rotate.
                }
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.Z)) //결정키
            {
                theAudio.Play(enterSound);
                keyInput = false;
                StartCoroutine(OXCoroutine());
            }
            else if (Input.GetKeyDown(KeyCode.X)) //취소키
            {
                theAudio.Play(cancelSound);
                keyInput = false;
                StartCoroutine(ExitCoroutine());
            }
        }
    }

    IEnumerator OXCoroutine() //정답이 맞는지 판별하는 코루틴.
    {
        //Color 연출
        Color color = numberText[0].color; //투명도 초기화를 위해.
        color.a = 1f;
        // e.g. 5000 >> 1356 입력 시, i=0부터 시작하면 6531이 저장됨.
        // 따라서 i=count부터 시작해 i--를 해나간다. (끝번호부터 저장) 
        for (int i=count; i>=0; i--)
        {
            numberText[i].color = Color;
            tempNumber += numberText[i].text;
        }
        yield return new WaitForSeconds(1f); // 대기

        // OX 판별
        // 저장한 숫자를 강제 형변환을 통해 Integer값으로 바꾸고, 정답(correctNumber)과 일치하는지 판별한다.
        result = int.Parse(tempNumber);
        if (result == correctNumber)
        {
            theAudio.Play(correctSound);
            correctFlag = true;
        } else
        {
            theAudio.Play(cancelSound);
            correctFlag = false;
        }

        Debug.Log("우리가 낸 답: "+ result + "\n정답: " + correctNumber); //Log를 활용해 Debug 시에 값을 확인할 수 있다.
        StartCoroutine(ExitCoroutine()); //판별이 끝났다면 ExitCoroutine을 호출해 종료한다.
    }

    IEnumerator ExitCoroutine()
    {
        //모두 초기화
        result = 0;
        tempNumber = "";
        anim.SetBool("Appear", false);
        yield return new WaitForSeconds(0.2f); // anim 종료 대기
        for (int i = 0; i <= count; i++) 
        {
            panel[i].SetActive(false);
            numberText[i].text = "0";
        }
        //뷰 위치 초기화
        superObject.transform.position = new Vector3(superObject.transform.position.x - position * count,
                                                     superObject.transform.position.y,
                                                     superObject.transform.position.z);
        activated(false);
    }
}
