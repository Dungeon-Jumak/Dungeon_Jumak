using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireTouch : MonoBehaviour
{
    public Text sizeText;
    public Image fireImage;
    private int touchCount = 0;

    public Fire fire;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            touchCount++;
            if (touchCount % 3 == 0 && fire.fireSize <= 100)
            {
                fire.IncreaseFireSize();
            }
        }
    }
}
