using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * CameraManager
 * ī�޶� ĳ���͸� ���� �� �ֵ��� ����. 
 */

public class CameraManager : MonoBehaviour
{
    #region singleton
    //�̱���ȭ(only one exists)�� ���� �ڱ� �ڽ��� ��ü�� ������ static ������ �����Ѵ�.
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

    public GameObject target; //ī�޶� ���� ���.
    public float moveSpeed; //ī�޶� �̵� �ӵ�.
    public Vector3 targetPosition; //ī�޶� ���� ����� ���� ��ġ��

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
        //Camera�� �� �����Ӹ��� target�� ���󰡾��ϱ� ������ Update �޼ҵ� ���ο� �ۼ��Ѵ�.
        if (target.gameObject != null) //target�� �����ϴ��� üũ
        {
            //targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z); // z���� ���� this(��ü ��ü)�� �����ؾ��ϴ� ����: camera�� target�� �������� ��ġ�� target���� �ڿ� �����ؾ��ϰ�, �� ���� ī�޶� ��ü�� ��ġ�� �������̱� �����̴�. 
            targetPosition.Set(target.transform.position.x, target.transform.position.y, -10); //this ��ü�� z���� 0���� �����Ǿ�����.
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime); // ī�޶� �̵�; lerp: (x, y, t); Time.deltaTime: 1/(1�ʿ� ����Ǵ� ������) : ��, 1�ʿ� moveSpeed��ŭ ������ x���� y�� �̵��ϰڴ�.
            
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
