using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NumberSystem
 * ���̾� �ý����� Ȱ���ϰ� ���� �̺�Ʈ���� object�� ����� NumberSystem class�� �̿��� �� �ִ�. 
 */

public class NumberSystem : MonoBehaviour
{
    // ���� : ����Ű, ����Ű, ���Ű �� �̺�Ʈ
    private AudioManager theAudio;
    public string keySound;
    public string enterSound;
    public string cancelSound;
    public string correctSound;

    private int count; // �迭�� ũ��. ��, �ڸ����� �ǹ��Ѵ�.
    private int selectedTextBox; // ���õ� �ڸ���. ��, ��� �ڽ��� ���õǾ������� �ǹ��Ѵ�. 
    private int result; // �÷��̾ ������ ��.
    private int correctNumber; //����.

    private string tempNumber;

    public GameObject superObject; // ���̾� object�� ��ġ�� (�ڸ����� ����) �����Ѵ�.
    public int position; //inspectorâ���� 30���� �����ϸ� ������ ��ġ�� �´�.
    public GameObject[] panel;
    public Text[] numberText;

    public Animator anim;

    public bool activated; // ��� (return new waitUntil)
    public bool keyInput; // Űó�� Ȱ��ȭ ����
    public bool correctFlag; // ���� ���� (result==correctNumber?)

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>(); 
    }

    // ���̾� ȭ�� ��. (Ȱ��ȭ)
    public void ShowNumber(int _correctNumber) 
    {
        //�ʱ�ȭ
        correctNumber = _correctNumber;
        activated = true;
        correctFlag = false;
        string temp = correctNumber.ToString(); //�ڸ��� ���� ��, Length ����� �̿��ϱ� ����.
        for (int i = 0; i < temp.Length; i++) {
            count = i; //count==temp.Lenght-1
            panel[i].SetActive(true);
            numberText[i].text = "0";
        }

        //�� ��ġ ����
        superObject.transform.position = new Vector3(superObject.transform.position.x + position*count, 
                                                     superObject.transform.position.y, 
                                                     superObject.transform.position.z);
        //�ʱ�ȭ
        selectedTextBox = 0;
        result = 0;
        SetColor(); 
        anim.SetBool("Appear", true);
        keyInput = true;
    }

    //���(���� ����) ��ȯ
    public bool GetResult()
    {
        return correctFlag;
    }

    //Update �޼ҵ� ���� keyInput(UP&DOWN) ó���� ���� �޼ҵ�.
    public void SetNumber(string _arrow)
    {
        int temp = int.Parse(numberText[selectedTextBox].text); //numberText[selectedTextBox].text�� Integer ������ ���� ����ȯ. (���� ����� ����)
        if (_arrow == "DOWN")
        {
            if (temp == 0)
            {
                temp = 9; //down Űó�� �� number���� 0�� �Ǹ�, 9�� �ٲ��ش�.
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
                temp = 0; //up Űó�� �� number���� 9�� �Ǹ�, 0�� �ٲ��ش�.
            }
            else
            {
                temp++;
            }
        }
        numberText[selectedTextBox].text = temp.ToString(); //�ٽ� String �������� ��ȯ���ش�.
    }

    public void SetColor()
    {
        Color color = numberText[0].color; //�г� �̹����� color�� �����´�.
        color.a = 0.3f; //alpha(����)�� 0.3f�� �����Ѵ�.
        for (int i = 0; i <= count; i++)
        {
            numberText[i].color = color;
        }
        color.a = 1f;
        numberText[selectedTextBox].color = color; //���õ� answer(result)�� color alpha 1f�� ������.
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
                    selectedTextBox = 0; //count�� �ڸ������� leftŰ �Է� ��, rotate.
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
                    selectedTextBox = count; //0�� �ڸ������� rightŰ �Է� ��, rotate.
                }
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.Z)) //����Ű
            {
                theAudio.Play(enterSound);
                keyInput = false;
                StartCoroutine(OXCoroutine());
            }
            else if (Input.GetKeyDown(KeyCode.X)) //���Ű
            {
                theAudio.Play(cancelSound);
                keyInput = false;
                StartCoroutine(ExitCoroutine());
            }
        }
    }

    IEnumerator OXCoroutine() //������ �´��� �Ǻ��ϴ� �ڷ�ƾ.
    {
        //Color ����
        Color color = numberText[0].color; //���� �ʱ�ȭ�� ����.
        color.a = 1f;
        // e.g. 5000 >> 1356 �Է� ��, i=0���� �����ϸ� 6531�� �����.
        // ���� i=count���� ������ i--�� �س�����. (����ȣ���� ����) 
        for (int i=count; i>=0; i--)
        {
            numberText[i].color = Color;
            tempNumber += numberText[i].text;
        }
        yield return new WaitForSeconds(1f); // ���

        // OX �Ǻ�
        // ������ ���ڸ� ���� ����ȯ�� ���� Integer������ �ٲٰ�, ����(correctNumber)�� ��ġ�ϴ��� �Ǻ��Ѵ�.
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

        Debug.Log("�츮�� �� ��: "+ result + "\n����: " + correctNumber); //Log�� Ȱ���� Debug �ÿ� ���� Ȯ���� �� �ִ�.
        StartCoroutine(ExitCoroutine()); //�Ǻ��� �����ٸ� ExitCoroutine�� ȣ���� �����Ѵ�.
    }

    IEnumerator ExitCoroutine()
    {
        //��� �ʱ�ȭ
        result = 0;
        tempNumber = "";
        anim.SetBool("Appear", false);
        yield return new WaitForSeconds(0.2f); // anim ���� ���
        for (int i = 0; i <= count; i++) 
        {
            panel[i].SetActive(false);
            numberText[i].text = "0";
        }
        //�� ��ġ �ʱ�ȭ
        superObject.transform.position = new Vector3(superObject.transform.position.x - position * count,
                                                     superObject.transform.position.y,
                                                     superObject.transform.position.z);
        activated(false);
    }
}
