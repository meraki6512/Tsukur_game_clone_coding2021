using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour {

    public string transferMapName;

    public Transform target;
    public BoxCollider2D targetBound;

    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade; //FadeOut FadeIn 함수를 쓰기 위해 선언
    private OrderManager theOrder; //맵이동 중에는 캐릭터의 이동 제한 NotMove(), Move()함수를 쓰기 위해 선언

	// Use this for initialization
	void Start () {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>(); 
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }
    }

    IEnumerator TransferCoroutine() //FadeOut과 FadeIn이 순식간에 일어나는것 방지 약간의 딜레이를 줌
    {
        theOrder.NotMove();
        theFade.FadeOut(); //화면 어두워짐
        yield return new WaitForSeconds(1f);
        thePlayer.currentMapName = transferMapName;
        theCamera.SetBound(targetBound);
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn(); //화면 밝아짐
        yield return new WaitForSeconds(0.5f);
        theOrder.Move();
    }

}
