﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

    public float cameraSpeedV = 2.0f;
    public float pitch = 0.0f;
	private float yaw = 0.0f;

	void Update () {
		var parent = gameObject.GetComponentInParent<CharacterMovement>();
		if (parent != null){
			yaw = gameObject.GetComponentInParent<CharacterMovement>().yaw;
		}
        pitch -= cameraSpeedV * Input.GetAxis("Mouse Y");
		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
	}
}