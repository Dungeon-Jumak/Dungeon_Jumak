//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class RotateKettle : MonoBehaviour
{
    //Min Rotation Value
    [Header("최소 회전 값")]
    [SerializeField] private float minRotation = 80f;

    //Max Rotation Value
    [Header("최대 회전 값")]
    [SerializeField] private float maxRotation = 180f;

    //Duration of Rotation
    [Header("한번 회전하는데 걸리는 시간")]
    [SerializeField] private float rotationDuration = 0.5f;

    //Current Rotation Angle
    private float currentRotation;

    //Current Rotation Direction
    private bool rotationDirection = true;

    void Start()
    {
        //Initialze Current Rotation
        currentRotation = minRotation;
    }

    void Update()
    {
        KettleRotation();
    }
    
    //For Rotate Kettle
    private void KettleRotation()
    {
        //Compute Step per frame
        float step = (maxRotation - minRotation) / rotationDuration * Time.deltaTime;

        //If positive direction
        if (rotationDirection)
        {
            //add step to current rotation
            currentRotation += step;

            //if arrive max rotation
            if (currentRotation >= maxRotation)
            {
                //Update current rotation
                currentRotation = maxRotation;

                //1 ~ 4 random rotration duration
                rotationDuration = Random.Range(0.3f, 0.7f);

                //Change Direction : positive -> negative
                rotationDirection = false;
            }
        }
        //If negative direction
        else
        {
            //sub step to current rotation
            currentRotation -= step;

            //if arrive min rotation
            if (currentRotation <= minRotation)
            {
                //Update current rotation
                currentRotation = minRotation;

                //1 ~ 4 random rotration duration
                rotationDuration = Random.Range(1f, 4f);

                //Change Direction : negative -> positive
                rotationDirection = true;
            }
        }

        //Update Euler Rotation
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }
}
