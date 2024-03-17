using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    Transform target;

    bool furnaceCheck = false;

    [Header("°Å¸®")]
    [SerializeField][Range(0f, 3f)] float contactDistance = 1f;

    public GameObject firePopUp;
    public GameObject PaJeonPopUp;
    public PaJeonManager paJeonManager;
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
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (touchPos.x < transform.position.x)
                {
                    PaJeonPopUp.gameObject.SetActive(true);
                    Debug.Log("¿ÞÂÊ ÅÍÄ¡µÊ");
                    
                }
                else
                {
                    firePopUp.gameObject.SetActive(true);
                    Debug.Log("¿À¸¥ÂÊ ÅÍÄ¡µÊ");
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
                    PaJeonPopUp.gameObject.SetActive(true);
                    Debug.Log("¿ÞÂÊ Å¬¸¯µÊ");
                    
                }
                else
                {
                    firePopUp.gameObject.SetActive(true);
                    Debug.Log("¿À¸¥ÂÊ Å¬¸¯µÊ");
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
