using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    public Transform cam;

    private Vector2 prevPos = Vector2.zero;
    private float prevDistance = 0;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        OnDrag();
    }

    public void OnDrag()
    {
        int touchCount = Input.touchCount;

        if(touchCount == 1)
        {
            if(prevPos == Vector2.zero)
            {
                prevPos = Input.GetTouch(0).position;
                return;
            }

            Vector2 dir = (Input.GetTouch(0).position - prevPos).normalized;
            Vector3 vec = new Vector3(dir.x, 0, dir.y);

            cam.position -= vec * moveSpeed * Time.deltaTime;
            prevPos = Input.GetTouch(0).position;
        }
    }
}
