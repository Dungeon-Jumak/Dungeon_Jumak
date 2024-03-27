using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject firePopUp;
    public GameObject paJeonPopUp;
    public PaJeonManager paJeonManager;

    public bool inputEnabled = true;

    private Transform target;
    private bool isNearFurnace = false;

    [Header("거리")]
    [SerializeField][Range(0f, 3f)] float contactDistance = 1f;



    void Start()
    {
        firePopUp.gameObject.SetActive(false);
        paJeonPopUp.gameObject.SetActive(false);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (isNearFurnace)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (!inputEnabled)
            return;

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 inputPosition = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (worldPosition.x < transform.position.x)
                {
                    PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

                    //---왼쪽을 클릭했을 때---//
                    if (playerMovement != null && !playerMovement.isCarryingFood)
                    {
                        paJeonPopUp.SetActive(true);
                        playerMovement.isCarryingFood = true;
                        inputEnabled = false;
                    }
                }
                else
                {
                    //---오른쪽을 클릭했을 때---//
                    firePopUp.SetActive(true);
                    inputEnabled = false;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        isNearFurnace = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isNearFurnace = false;
    }
}
