using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour {

    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer; //animator.getFloat "DirY" == 1f
    private FadeManager theFade;

    private bool flag = false;//이벤트를 한번만 발생시키기 위해 사용

	// Use this for initialization
	void Start () {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
	}

    private void OnTriggerStay2D(Collider2D collision) //박스안에 캐릭터가 있을 경우 계속 실행
    {
        if (!flag && Input.GetKey(KeyCode.Z) && thePlayer.animator.GetFloat("DirY") == 1f) //z키가 눌렸을 경우, 캐릭터가 보는 방향이 위일 경우(위로 움직이는 방향키를 눌렀을 경우)
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter(); //캐릭터를 불러온다.

        theOrder.NotMove(); // 키보드로 이동하는 움직임 제한

        theDM.ShowDialogue(dialogue_1);

        yield return new WaitUntil(() => !theDM.talking); //대화하는데 걸리는 시간을 예측 할 수 없기에 대화가 끝나면 이동을 진행한다.

        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "UP");

        yield return new WaitUntil(() => thePlayer.queue.Count == 0); //이동이 끝나면 움직임 //queue에 Right, Right, UP이 들어가고 이동하면서 하나씩 빠져나오게된다. 즉, 큐가 비었다 = 이동이 끝났다.

        theFade.Flash();
        theDM.ShowDialogue(dialogue_2);

        yield return new WaitUntil(() => !theDM.talking); //대화하는데 걸리는 시간을 예측 할 수 없기에 대화가 끝나면 이동을 진행한다.

        theOrder.Move();
    }

}
