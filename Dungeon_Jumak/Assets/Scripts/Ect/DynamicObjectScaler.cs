using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectScaler : MonoBehaviour
{
    private float height = 1.92f;

    private void Start()
    {
        float newWidth = ((float)Screen.width / Screen.height) * height;

        transform.localScale = new Vector3(newWidth, height, transform.localScale.z);
        transform.position = Vector3.zero;

        AstarPath.active.Scan();
    }
}
