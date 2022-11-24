using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour {

    private PlayerManager thePlayer; //이벤트 도중에 키입력 처리 방지

    private List<MovingObject> characters; //캐릭터를 담는다.

	// Use this for initialization
	void Start () {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    public void PreLoadCharacter() //리스트에 캐릭터를 채워넣음
    {
        characters = ToList(); //characters리스트에 ToList()의 반환값을 넣어줌
    }

    public List<MovingObject> ToList()
    {
        List<MovingObject> tempList = new List<MovingObject>();
        MovingObject[] temp = FindObjectsOfType<MovingObject>(); //Objects : MovingObject가 달린 모든 객체를 찾아서 반환함

        for (int i = 0; i < temp.Length; i++) //배열의 크기만큼 반복하며 tempList에 대입
        {
            tempList.Add(temp[i]);
        }

        return tempList;
    }

    public void NotMove() //움직임 제한
    {
        thePlayer.notMove = true;
    }

    public void Move() //움직임 허용
    {
        thePlayer.notMove = false;
    }

    public void SetThorought(string _name) //벽 뚫기 허용
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].boxCollider.enabled = false;
            }
        }
    }

    public void SetUnThorought(string _name)//벽 뚫기 비허용
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].boxCollider.enabled = true;
            }
        }
    }

    public void SetTransparent(string _name)//투명화
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetUnTransparent(string _name) //투명화 해제
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(true);
            }
        }
    }

    public void Move(string _name, string _dir) // 이동함수 _name : 움직일 캐릭터 이름, _dir : 캐릭터가 움직일 방향
    {
        for (int i = 0; i < characters.Count; i++) //배열에 들어있는 캐릭터의 수 만큼 반복
        {
            if (_name == characters[i].characterName)
            {
                characters[i].Move(_dir);
            }
        }
    }

    public void Turn(string _name, string _dir) //방향을 바라보도록하는 함수 _name : 움직일 캐릭터 이름, _dir : 캐릭터가 바라볼 방향
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].animator.SetFloat("DirX", 0f);
                characters[i].animator.SetFloat("DirY", 0f);

                switch (_dir)
                {
                    case "UP":
                        characters[i].animator.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        characters[i].animator.SetFloat("DirY", -1f);
                        break;
                    case "RIGHT":
                        characters[i].animator.SetFloat("DirX", 1f);
                        break;
                    case "LEFT":
                        characters[i].animator.SetFloat("DirX", -1f);
                        break;
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
