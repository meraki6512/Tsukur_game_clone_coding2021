using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Text를 사용하기 위해 필요

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance; // DialogueManager는 하나만 필요하기 때문에 싱글톤화를 시킨다. 싱글톤 패턴 - 객체를 하나만 생성하도록함

    private void Awake() // void Start() 보다 먼저 실행되며 싱글톤화를 시킨다.
    {
        if (instance == null)
        {
 
            DontDestroyOnLoad(this.gameObject);
           instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Text text; //대사를 교체하는데 사용할 변수
    // sprite - audioclip - mp3, spriternderer - audiosource - mp3플레이어
    public SpriteRenderer rendererSprite; //SpriteRenderer : 이미지를 화면에 그려줌 
    public SpriteRenderer rendererDialogueWindow;

    //대사마다 크기가 달라질 수 있기에 리스트 사용
    public List<string> listSentences; // 커스텀 클래스 Dialogue에서 만든 배열을 리스트에 넣는다.
    public List<Sprite> listSprites; //
    public List<Sprite> listDialogueWindows; // 

    private int count; //대화 진행 상황 카운트

    //애니메이션 통제
    public Animator animSprite; //대상
    public Animator animDialogueWindow; //대화창

    public string typeSound;
    public string enterSound;

    private AudioManager theAudio;

    public bool talking = false; //대화를 하지 않을 때는 Z의 입력을 막는다.
    private bool keyActivated = false; //z키의 입력을 연달아 받을 수 없게 제한


    // Use this for initialization
    void Start () {
        theAudio = FindObjectOfType<AudioManager>(); //AudioManager를 불러옴
        //리스트 및 변수 초기화
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
	}
	
    public void ShowDialogue(Dialogue dialogue) //대화창(dialogue)가 나오기 위한 함수
    {
        talking = true;

        for(int i = 0; i < dialogue.sentances.Length; i++)
        {
            listSentences.Add(dialogue.sentances[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindows[i]);
        }


        animSprite.SetBool("Appear", true); //애니메이션을 화면안으로 불러옴
        animDialogueWindow.SetBool("Appear", true); //  ;;
        StartCoroutine(StartDialogueCoroutine()); //대화 실행
    }

    public void ExitDialogue()//대화창 종료
    {
        //변수, 리스트 초기화
        text.text = "";
        count = 0;
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        animSprite.SetBool("Appear", false); //이미지 사라짐
        animDialogueWindow.SetBool("Appear", false); //대화창 사라짐
        talking = false;
    }

    //coroutine : update와 비슷한 기능을 함, update는 매 프레임마다 이루어지지만 coroutine은 한 프레임 안에서 이루어진다.
    //yield return new WaitForSeconds(시간); 지정된 시간 후에 다음 프레임에서 재개
    IEnumerator StartDialogueCoroutine() //대화창을 보여주는 coroutine
    {
        if(count > 0)// count가 0일 경우 문제 발생
        {
           if(listDialogueWindows[count] != listDialogueWindows[count - 1])//두개의 대화창이 다르다면 교체한다. 대화창이 다름 = 인물이 달라짐
            {
                animSprite.SetBool("Change", true);
                animDialogueWindow.SetBool("Appear", false);
                yield return new WaitForSeconds(0.2f);
                rendererDialogueWindow.sprite = listDialogueWindows[count]; //대화창 변경
                rendererSprite.sprite = listSprites[count]; 
                animDialogueWindow.SetBool("Appear", true);
                animSprite.SetBool("Change", false);
            }
            else//대화장이 같을 경우
            {
                //이미지 교체 확인
                if(listSprites[count] != listSprites[count - 1]) //두개의 이미지가 다르다면 교체한다.
                {
                    animSprite.SetBool("Change", true);
                    yield return new WaitForSeconds(0.1f);//대기
                    rendererSprite.sprite = listSprites[count];//getComponent를 이용하여 spirte속성을 사용하여 listSprites[count]의 내용을 rendererSprite에 저장
                    animSprite.SetBool("Change", false);
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        else // 첫 이미지는 반드시 교체됨
        {
            rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
        }

        keyActivated = true; 

        //text출력 담당
        for (int i = 0; i < listSentences[count].Length; i++)//count번째 문장의 길이만큼 i를 반복
        {
            text.text += listSentences[count][i]; //한글자 씩 출력
            if(i%7 == 1)
            {
                theAudio.Play(typeSound); //음성 재생
            }
            yield return new WaitForSeconds(0.01f);//대기
        }

    }

	// Update is called once per frame
	void Update () {
        if(talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.Z)) // Z키가 눌렸을 경우 실행
            {
                keyActivated = false; //입력되었기 때문에 제한
                text.text = ""; //
                count++; // Z키가 눌림 -> 첫번째 문장을 읽음 따라서 count가 증가
                theAudio.Play(enterSound); //enter키가 눌리면 소리 나옴

                if (count == listSentences.Count) //count가 listSentences의 갯수와 똑같을 경우 저장된 대사가 모두 출력됬다는 의미
                {
                    StopAllCoroutines(); //모든 coroutine 정지
                    ExitDialogue(); //대화창 종료
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartDialogueCoroutine());
                }

            }
        }
	}
}
