using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * LightController
 * Ư�� event���� Object�� ����� LightController�� Ȱ��ȭ�� �پ��� ��Ȳ�� ������ �� �ִ�. 
 */

public class LightController : MonoBehaviour
{

    //�÷��̾ �ٶ󺸰� �ִ� ����. -> animator.getfloat
    private PlayerManager thePlayer;
    private Vector2 vector;

    //ȸ��(����)�� ����ϴ� Vector4
    private Quaternion rotation;


    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfTyper<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = thePlayer.transform.position; // �������� ĳ���Ϳ� �پ��ִ� ����

        vector.Set(thePlayer.Animator.GetFloat("DirX"), thePlayer.Animator.GetFloat("DirY"));
        if (vector.x == 1f)
        { // right
            rotation = Quaternion.Euler(0, 0, 90);
            this.transform.rotation = rotation;
        }
        else if (vector.x == -1f)
        { // left
            rotation = Quaternion.Euler(0, 0, -90);
            this.transform.rotation = rotation;
        }
        else if (vector.y == 1f)
        { // up 
            rotation = Quaternion.Euler(0, 0, 180);
            this.transform.rotation = rotation;
        }
        else if (vector.y == -1f) 
        { // down
            rotation = Quaternion.Euler(0, 0, 0);
            this.transform.rotation = rotation;
        }
    }
}
