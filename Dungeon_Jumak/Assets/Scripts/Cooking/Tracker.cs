using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    Transform target;

    bool furnaceCheck = false;

    [Header("�Ÿ�")]
    [SerializeField][Range(0f, 3f)] float contactDistance = 1f;

    public GameObject firePopUp;
    public GameObject PaJeonPopUp;
    public PaJeonManager paJeonManager;

    public bool inputEnabled = true;


    void Start()
    {
        firePopUp.gameObject.SetActive(false);
        PaJeonPopUp.gameObject.SetActive(false);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        CheckTarget();
    }

    void CheckInput()
    {
        if (!inputEnabled) // �̺�Ʈ�� ��Ȱ��ȭ�Ǿ� ������ �Լ��� �ٷ� �����մϴ�.
            return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (touchPos.x < transform.position.x)
                {
                    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                    PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
                    if (playerMovement.isCarryingFood == false)
                    {
                        PaJeonPopUp.gameObject.SetActive(true);
                        Debug.Log("���� ��ġ��");
                        playerMovement.isCarryingFood = true;
                        inputEnabled = false;

                    }
                    
                }
                else
                {
                    firePopUp.gameObject.SetActive(true);
                    Debug.Log("������ ��ġ��");
                    

                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (mousePos.x < transform.position.x)
                {
                    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                    PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
                    if (playerMovement.isCarryingFood == false)
                    {
                        PaJeonPopUp.gameObject.SetActive(true);
                        Debug.Log("���� ��ġ��");
                        playerMovement.isCarryingFood = true;
                        inputEnabled = false;
                    }

                }
                else
                {
                    firePopUp.gameObject.SetActive(true);
                    Debug.Log("������ Ŭ����");
                    
                }
            }
        }
    }


    void CheckTarget()
    {
        if (Vector2.Distance(transform.position, target.position) > contactDistance && furnaceCheck)
        {
            CheckInput();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        furnaceCheck = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        furnaceCheck = false;
    }
}
