using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    private GameObject cam;

    public float parallaxeffect; //背景比镜头慢的倍速
    private float xPosition;
    private float length;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        //xPosition = cam.transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxeffect;
        float distanceMoved = cam.transform.position.x * (1 - parallaxeffect);
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        if (distanceMoved > xPosition + length)
        {
            xPosition += length;
        }
        else if (distanceMoved < xPosition - length)
        {
            xPosition -= length;
        }
    }
}
