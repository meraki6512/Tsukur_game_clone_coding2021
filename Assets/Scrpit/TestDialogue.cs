using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour {
    [SerializeField]
    public Dialogue dialogue;

    private DialogueManager theDM;

	// Use this for initialization
	void Start () {
        theDM = FindObjectOfType<DialogueManager>();
	}

    private void OnTriggerEnter2D(Collider2D collision) //boxcollider에 충돌할 때 인식
    {
        if(collision.gameObject.name == "Player") //출동한 대상이 Player일 경우 실행
        {
            theDM.ShowDialogue(dialogue);
        }
    }

}
