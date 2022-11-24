using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * LightController
 * 특정 event에서 Object를 만들어 LightController를 활성화해 다양한 상황을 연출할 수 있다. 
 */

public class LightController : MonoBehaviour
{

    //플레이어가 바라보고 있는 방향. -> animator.getfloat
    private PlayerManager thePlayer;
    private Vector2 vector;

    //회전(각도)을 담당하는 Vector4
    private Quaternion rotation;


    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfTyper<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = thePlayer.transform.position; // 손전등이 캐릭터와 붙어있는 연출

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
