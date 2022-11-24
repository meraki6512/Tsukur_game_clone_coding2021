using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * DB Manager
 * .csv/.xlsx/.xml 등을 활용하면, 보거나 관리하기에 더 쉽지만, 게임 프로젝트의 규모가 작으므로, script를 이용해 구현한다.
 * 1) Scene 이동 : scene 이동 시(A<->B), objects가 파괴되면서 scripts가 모두 초기화된다. 예를 들어, A의 같은 event가 (원하지 않아도) 여러 번 반복 실행될 수 있다. 
 * 2) Save와 Load
 * 3) Item 등을 미리 만들어둘 수 있음.
 */

public class DatabaseManager : MonoBehaviour
{

    //(data) objects를 담는 배열 선언
    public string[] var_name;
    public float[] var;
    public string[] switch_name;
    public bool[] switches;

    //싱글턴화(only one exists)를 위한 자기 자신을 객체로 가지는 static 변수를 생성한다.
    static public DatabaseManager instance;

    //싱글턴화 과정 (Scene 이동 시, DatabaseManger Destroy를 막는다.)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame -> 불필요
}
