using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

	public Transform target;

	public float time;

	private float sunTime;

	const float PI = 3.1415926f;

	private float xOffset;
	private Vector2 sunRotation;
	private float runRadius;

	// Use this for initialization
	void Start () {
		time = 12;
		transform.LookAt(target);
		xOffset = transform.position.x;
		sunRotation = new Vector2(transform.position.z, transform.position.y);
		runRadius = sunRotation.magnitude;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		sunTime = time*PI/12f;
		sunRotation = new Vector2(Mathf.Cos(sunTime), Mathf.Sin(sunTime)) * runRadius;

		transform.position = new Vector3(xOffset, sunRotation.y, sunRotation.x);
		transform.LookAt(target);
	}
}
