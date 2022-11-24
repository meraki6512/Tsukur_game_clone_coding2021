using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * DB Manager
 * .csv/.xlsx/.xml ���� Ȱ���ϸ�, ���ų� �����ϱ⿡ �� ������, ���� ������Ʈ�� �Ը� �����Ƿ�, script�� �̿��� �����Ѵ�.
 * 1) Scene �̵� : scene �̵� ��(A<->B), objects�� �ı��Ǹ鼭 scripts�� ��� �ʱ�ȭ�ȴ�. ���� ���, A�� ���� event�� (������ �ʾƵ�) ���� �� �ݺ� ����� �� �ִ�. 
 * 2) Save�� Load
 * 3) Item ���� �̸� ������ �� ����.
 */

public class DatabaseManager : MonoBehaviour
{

    //(data) objects�� ��� �迭 ����
    public string[] var_name;
    public float[] var;
    public string[] switch_name;
    public bool[] switches;

    //�̱���ȭ(only one exists)�� ���� �ڱ� �ڽ��� ��ü�� ������ static ������ �����Ѵ�.
    static public DatabaseManager instance;

    //�̱���ȭ ���� (Scene �̵� ��, DatabaseManger Destroy�� ���´�.)
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

    // Update is called once per frame -> ���ʿ�
}
