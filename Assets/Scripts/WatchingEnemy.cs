using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchingEnemy : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float visionDistance;
    [SerializeField] LineRenderer los;

    void Update()
    {
        los.SetPosition(0,transform.position);
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, visionDistance);
       if(hitInfo.collider != null)
       {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            los.SetPosition(1, hitInfo.point);
            los.startColor= Color.red;
            los.endColor= Color.red;
        }
       else
       {
            Debug.DrawLine(transform.position, transform.position+ transform.right* visionDistance, Color.green);
            los.SetPosition(1, transform.position + transform.right * visionDistance);
            los.startColor = Color.green;
            los.endColor = Color.green;
        }
    }
}
