using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    Rigidbody2D rb;
    Transform target;

    bool furnaceCheck = false;

    [Header ("거리")]
    [SerializeField][Range(0f, 3f)] float contactDistance = 1f;

    public GameObject furnacePopUp;
    void Start()
    {
        furnacePopUp.gameObject.SetActive(false);
        //rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        checkTarget();
    }

    private void checkTarget()
    {
        if(Vector2.Distance(transform.position, target.position) > contactDistance && furnaceCheck)
        {
            if(Input.GetKey(KeyCode.L))
            {
                furnacePopUp.gameObject.SetActive(true);
            }
            Debug.Log("범위 안");
        }
        else
        {
            Debug.Log("범위 밖");
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
