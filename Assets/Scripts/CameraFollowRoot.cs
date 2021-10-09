using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowRoot : MonoBehaviour
{
    private float _initialYPosition;
    private float _greatestXPosition;

    private void Start()
    {
        _initialYPosition = transform.position.y;
        // _greatestXPosition = transform.position.x;
    }

    private void Update()
    {
        // Debug.Log("CameraFollowRoot: " + transform.position.x);
        // Debug.Log("Player " + transform.parent.position.x);
        // if (transform.position.x > _greatestXPosition)
        // {
        //     _greatestXPosition = transform.position.x;
        // }
        // if (transform.position.x > _greatestXPosition)
        // {
        //     _greatestXPosition = transform.position.x;
        // }
        // if (transform.parent.position.x > _greatestXPosition)
        // {

        // }
        var position = transform.position;
        position.y = _initialYPosition;
        // position.x = _greatestXPosition;
        transform.position = position;
    }
}
