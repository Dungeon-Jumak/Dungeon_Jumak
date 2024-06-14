//System
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class CursorController : MonoBehaviour
{
    //Drag Speed
    [SerializeField] private float dragSpeed;

    //First Click Point
    private Vector3 firstClickPoint;

    private void Update()
    {
        ViewMoving();
    }

    void ViewMoving()
    {
        //Check First Mouse Position
        if (Input.GetMouseButtonDown(0))
            firstClickPoint = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

        //Mouse Button Continious
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(- new Vector3(Input.mousePosition.x - firstClickPoint.x, Input.mousePosition.y - firstClickPoint.y, Camera.main.transform.position.z));

            Vector3 move = new Vector3(pos.x * dragSpeed * Time.deltaTime, pos.y * dragSpeed * Time.deltaTime, pos.z);


            Camera.main.transform.Translate(move);
        }
    }

}
