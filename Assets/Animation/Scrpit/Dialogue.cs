using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 커스텀 클래스를 inspector 창에 띄울 수 있도록하게 만든다.
public class Dialogue {
    [TextArea(1, 2)]
    public string[] sentances;// 대화를 나누는 문장
    public Sprite[] sprites; //대사를 말하는 대상  //Sprite : 2D그래픽 오브젝트 
    public Sprite[] dialogueWindows; //말하는 사람의 대화창 이름

}
