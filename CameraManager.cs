using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * CameraManager
 * 카메라가 캐릭터를 따라갈 수 있도록 연출. 
 */

public class CameraManager : MonoBehaviour
{
    #region singleton
    //싱글턴화(only one exists)를 위한 자기 자신을 객체로 가지는 static 변수를 생성한다.
    static CameraManager instance;
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
    #endregion singleton

    public GameObject target; //카메라가 따라갈 대상.
    public float moveSpeed; //카메라 이동 속도.
    public Vector3 targetPosition; //카메라가 따라갈 대상의 현재 위치값

    public BoxCollider2D bound;

    private Vector3 minBound;
    private Vector3 maxBound;

    private float halfWidth;
    private float halfHeight;

    private Camera theCamera;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        //Camera는 매 프레임마다 target을 따라가야하기 때문에 Update 메소드 내부에 작성한다.
        if (target.gameObject != null) //target이 존재하는지 체크
        {
            //targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z); // z축의 값을 this(객체 자체)로 설정해야하는 이유: camera가 target을 찍으려면 위치상 target보다 뒤에 존재해야하고, 그 값이 카메라 객체의 위치로 고정적이기 때문이다. 
            targetPosition.Set(target.transform.position.x, target.transform.position.y, -10); //this 객체는 z값이 0으로 설정되어있음.
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime); // 카메라 이동; lerp: (x, y, t); Time.deltaTime: 1/(1초에 실행되는 프레임) : 즉, 1초에 moveSpeed만큼 움직여 x에서 y로 이동하겠다.
            
            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float ClampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
            this.transform.position = new Vector3(clampedX, ClampedY, -10);
        }
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
