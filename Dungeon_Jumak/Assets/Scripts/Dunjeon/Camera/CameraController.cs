//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Player : Target
    [SerializeField] Transform player;

    //Smoothing
    [SerializeField] float smoothing = 0.2f;

    //Bound's Boxcollider
    [SerializeField] private BoxCollider2D bound;

    //Min Bound
    private Vector3 minBound;

    //Max Bound
    private Vector3 maxBound;

    //Camera Component
    private Camera _camera;

    //To Compute
    private float halfWidth;
    private float halfHeight;

    private void Start()
    {
        _camera = GetComponent<Camera>();

        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

        halfHeight = _camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);

        float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
    }
}
