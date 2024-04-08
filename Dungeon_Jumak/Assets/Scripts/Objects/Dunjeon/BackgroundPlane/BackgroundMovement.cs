using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private int startIndex;

    [SerializeField]
    public int endIndex;

    [SerializeField]
    private Transform[] sprites;

    //private float viewHeight;

    /*private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }*/

    void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.back * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if (sprites[endIndex].position.z < -2)
        {
            Vector3 backSpritePos = sprites[startIndex].position;
            Vector3 frontSpritePos = sprites[endIndex].position;
            sprites[endIndex].transform.position = backSpritePos + Vector3.back * (-5);

            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave-1 == -1) ? sprites.Length-1 : startIndexSave - 1;
        }
    }
}
