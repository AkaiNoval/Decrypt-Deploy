using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    Vector3 touchStart;
    [Header("Pan Settings")]
    [SerializeField] float minXBound;
    [SerializeField] float maxXBound;
    [Header("Zoom Settings")]
    [SerializeField] float zoomMin;
    [SerializeField] float zoomMax;
    [SerializeField] float zoomSpeed;
    [SerializeField] float PinchZoomSpeed;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            ZoomWithPinch();
        }
        else if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);

                mainCamera.transform.position += new Vector3(direction.x, 0f, 0f);
                mainCamera.transform.position = new Vector3(Mathf.Clamp(mainCamera.transform.position.x, minXBound, maxXBound), mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
        }
        ZoomWithMouseScrollWheel(Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);
    }
    void ZoomWithMouseScrollWheel(float increment)
    {
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - increment,zoomMin,zoomMax);
    }

    void ZoomWithPinch()
    {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            ZoomWithMouseScrollWheel(difference * PinchZoomSpeed);
        
    }
}
