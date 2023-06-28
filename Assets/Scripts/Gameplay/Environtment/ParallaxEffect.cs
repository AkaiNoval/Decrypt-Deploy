using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    float length;
    float startPosX;
    public GameObject cam;
    public float parallaxEffectSpeed;
    void Start()
    {
        startPosX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void Update()
    {
        float distanceX = cam.transform.position.x * parallaxEffectSpeed;   
        transform.position = new Vector3(startPosX + distanceX, transform.position.y, transform.position.z);

        //How far we moved relative to the camera
        float temp = cam.transform.position.x *  (1 - parallaxEffectSpeed);
        if(temp > startPosX + length) 
        {
            startPosX += length;
        }
        else if(temp < startPosX - length) 
        {
            startPosX -= length;
        }
    }
}
