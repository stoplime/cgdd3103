using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public float cameraSpeedH = 2.0f;
	public float speedH = 2.0f;
    public float speedV = 2.0f;

    public float yaw = 0.0f;
	// Update is called once per frame
	void Update () {
		float xpos = Input.GetAxis("Horizontal2") * Time.deltaTime * speedH;
		float zpos = Input.GetAxis("Vertical2") * Time.deltaTime * speedV;

		transform.Translate(xpos, 0, zpos);

		yaw += cameraSpeedH * Input.GetAxis("lookAround2");

        transform.eulerAngles = new Vector3(0f, yaw, 0.0f);
	}
}
