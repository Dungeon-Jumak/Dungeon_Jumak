using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PlayerInWatingScene : MonoBehaviour
{
    //Player animator variable
    [SerializeField]
    private Animator animator;

    //player translation variables
    [SerializeField]
    private Vector3 curPosition;
    [SerializeField]
    private bool isMove = false;
    [SerializeField]
    private float speed;

    //Jumak And Dunjeon sign variables
    [SerializeField]
    private Transform jumakSign;
    [SerializeField]
    private Transform dungeonSign;

    
    [SerializeField]
    private Transform targetTransform;

    void Start()
    {
        animator = GetComponent<Animator>(); //Animator setting

        curPosition = new Vector3(0f, -3.8f); //current position setting
        isMove = false; //isMove setting
        speed = 3f; //speed setting
    }

    void Update()
    {
        //If bool variable isMove is true, move to target
        if (isMove)
        {
            MoveToTarget();
        }
    }

    //Go to the selected target location
    private void MoveToTarget()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(curPosition, targetTransform.position, step); // Player moving by MoveTowards

        curPosition = transform.position;//Set the current position to curPosition variable
    }

    //Go to the JumakSign position
    public void MoveJumak()
    {
        isMove = true;//Set isMove variable
        animator.SetInteger("DirX", -1);//Set animator

        targetTransform = jumakSign.transform;
    }

    //Go to the Dunjeon position
    public void MoveDungeon()
    {
        isMove = true;//Set isMove variable
        animator.SetInteger("DirX", 1);//Set animator

        targetTransform = dungeonSign.transform;
    }

    //Go to the Market position
    /*public void MoveMarket()
    {
        isMove = true;
    }*/

    //OnTrigger function to changing the scene
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target_Jumak")) GameManager.Scene.LoadScene(Define.Scene.Jumak);
        else if(other.CompareTag("Target_Dunjeon")) GameManager.Scene.LoadScene(Define.Scene.Map);
        //else if(col.CompareTag("Target_Market")) GameManager.Scene.LoadScene(Define.Scene.Market);
    }
}