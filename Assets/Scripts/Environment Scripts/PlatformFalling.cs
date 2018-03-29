﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFalling : MonoBehaviour {

    bool isFalling = false;
    float fallSpeed = 0;
    //transform gameObject;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            isFalling = true;
    }

    void Update()
    {
        if (isFalling)
        {
            fallSpeed += Time.deltaTime /150;
            transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, transform.position.z);
        }
    }
}