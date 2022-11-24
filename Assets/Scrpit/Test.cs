using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestMove
{
    public string name;
    public string direction;
}

public class Test : MonoBehaviour {

    [SerializeField]
    public TestMove[] move;

    private OrderManager theOrder;

	// Use this for initialization
	void Start () {
        theOrder = FindObjectOfType<OrderManager>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        theOrder.PreLoadCharacter();//캐릭터 정보불러오기
        if(collision.gameObject.name == "Player")  //박스에 충돌한 캐릭터의 이름이 Player일 때 실행
        {
            for(int i = 0; i<move.Length; i++)//미리 지정해논 move의 길이만큼 반복
            {
                theOrder.Move(move[i].name, move[i].direction);
            }
        }
    }

}
